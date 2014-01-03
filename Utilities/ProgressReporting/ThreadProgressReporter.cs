using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    internal sealed class ThreadProgressReporter
    {
        private readonly List<IProgressObserver> _mainProgressObservers;

        public ProgressReportScope MainScope { get; private set; }

        private ProgressReportScope MostNestedScope
        {
            get
            {
                ProgressReportScope scope = MainScope;
                while (scope.ChildScope != null) scope = scope.ChildScope;
                return scope;
            }
        }

        public ThreadProgressReporter()
        {
            _mainProgressObservers = new List<IProgressObserver>();
        }

        public ProgressReportScope CreateMainScope(int stepsCount, string name)
        {
            MainScope = new ProgressReportScope(stepsCount, name);
            _mainProgressObservers.ForEach(o => MainScope.Subscribe(o));
            _mainProgressObservers.Clear();
            MainScope.Subscribe(new ProgressObserver(onComplete: () => MainScope = null));
            return MainScope;
        }

        public ProgressReportScope CreateChildScope(int stepsCount, double span, string name)
        {
            if (MainScope == null)
                throw new InvalidOperationException("Main scope is not created.");

            ProgressReportScope scope = MostNestedScope;
            scope = scope.CreateChildScope(stepsCount, span, name);
            return scope;
        }

        public void IncreaseMostNestedScope()
        {
            MostNestedScope.Increase();
        }

        public void CompleteMostNestedScope()
        {
            MostNestedScope.Complete();
        }

        public void Subscribe(IProgressObserver progressObserver)
        {
            if (MainScope == null)
            {
                _mainProgressObservers.Add(progressObserver);
            }
            else
            {
                MainScope.Subscribe(progressObserver);    
            }
        }
    }
}