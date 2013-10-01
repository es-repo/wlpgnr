using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public sealed class ProgressReportScope : IDisposable, IObservable<double>, IObserver<double>
    {
        private double _progress;
        private double _previouseProgress;
        private bool _isCompleted;
        private readonly List<IObserver<double>> _progressObservers;
        private IDisposable _childScopeUnsubscriber;

        public double ChildScopeSpan { get; private set; }

        public ProgressReportScope ChildScope { get; private set; }

        public string Name { get; private set; }

        public int StepsCount { get; private set; }

        public double StepSize { get; private set; }

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (value > 1)
                    throw new InvalidOperationException(GetExceptionMessage(string.Format("Attempt to setup progress with value \"{0}\" more then 1.", value)));
                _progress = value;
                _progressObservers.ForEach(o => o.OnNext(_progress));
            }
        }

        public ProgressReportScope(int stepsCount = 1, [CallerMemberName] string name = "")
        {
            Name = name;
            StepsCount = stepsCount;
            StepSize = 1.0/stepsCount;
            _progressObservers = new List<IObserver<double>>();
        }

        public ProgressReportScope CreateChildScope(double span, [CallerMemberName] string name = "")
        {
            return CreateChildScope(1, span, name);
        }

        public ProgressReportScope CreateChildScope(int stepsCount, [CallerMemberName] string name = "")
        {
            return CreateChildScope(stepsCount, 1, name);
        }

        public ProgressReportScope CreateChildScope(int stepsCount, double span, [CallerMemberName] string name = "") 
        {
            if (ChildScope != null)
                throw new InvalidOperationException(GetExceptionMessage("Child scope is alredy created and not completed yet."));

            if (span <= 0)
                throw new ArgumentException(GetExceptionMessage("ChildScopeSpan should be greater then 0."), "span");

            ChildScopeSpan = span * StepSize;

            if (Progress + ChildScopeSpan > 1)
                throw new ArgumentException(GetExceptionMessage("Child scope span plus current progress is more then 1."), "span");
            
            ChildScope = new ProgressReportScope(stepsCount, name);
            _previouseProgress = Progress;
            _childScopeUnsubscriber = ChildScope.Subscribe(this);
            return ChildScope;
        }

        public void Increase()
        {
            Progress = _previouseProgress + StepSize;
            _previouseProgress = Progress;
        }

        public void Complete()
        {
            if (_isCompleted)
                throw new InvalidOperationException(GetExceptionMessage("Progress scope is already completed."));

            if (ChildScope != null)
                ChildScope.Complete();

            Progress = 1;
            _isCompleted = true;

            _progressObservers.ForEach(o => o.OnCompleted());
        }

        public ProgressFullInfo GetProgressFullInfo()
        {
            return new ProgressFullInfo(EnumerateAllProgresses());
        }

        private IEnumerable<ScopeProgress> EnumerateAllProgresses()
        {
            ProgressReportScope scope = this;
            while (scope != null)
            {
                yield return new ScopeProgress(scope.Name, scope.Progress);
                scope = scope.ChildScope;
            }
        }
        
        public void Dispose()
        {
            Complete();
        }

        public IDisposable Subscribe(IObserver<double> progressObserver)
        {
            if (progressObserver == null)
                throw new ArgumentNullException("progressObserver");

            _progressObservers.Add(progressObserver);
            return new Disposable(() => _progressObservers.Remove(progressObserver));
        }

        void IObserver<double>.OnNext(double value)
        {
            Progress = _previouseProgress + value * ChildScopeSpan;
        }

        void IObserver<double>.OnError(Exception error)
        {
        }

        void IObserver<double>.OnCompleted()
        {
            ChildScope = null;
            _childScopeUnsubscriber.Dispose();
            _childScopeUnsubscriber = null;
            Progress = _previouseProgress + ChildScopeSpan;
        }

        private string GetExceptionMessage(string messageBase)
        {
            return string.Format("{0}=[Name:\"{1}\"]. {2}", GetType().Name, Name, messageBase);
        }
    }
}
