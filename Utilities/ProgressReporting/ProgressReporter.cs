using System.Runtime.CompilerServices;
using System.Threading;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public static class ProgressReporter
    {
        private readonly static ThreadLocal<ThreadProgressReporter> _threadProgressReporter = new ThreadLocal<ThreadProgressReporter>(() => new ThreadProgressReporter());

        public static ProgressReportScope CreateScope([CallerMemberName] string name = "")
        {
            return CreateScope(1, 1, 0, name);
        }

        public static ProgressReportScope CreateScope(double span, [CallerMemberName] string name = "")
        {
            return CreateScope(1, span, 0, name);
        }

        public static ProgressReportScope CreateScope(double span, double initProgress, [CallerMemberName] string name = "")
        {
            return CreateScope(1, span, initProgress, name);
        }

        public static ProgressReportScope CreateScope(int stepsCount, [CallerMemberName] string name = "")
        {
            return CreateScope(stepsCount, 1, 0, name);
        }

        public static ProgressReportScope CreateScope(int stepsCount, double span, double initProgress = 0, [CallerMemberName] string name = "")
        {
            return _threadProgressReporter.Value.MainScope == null
                ? _threadProgressReporter.Value.CreateMainScope(stepsCount, span, initProgress, name)
                : _threadProgressReporter.Value.CreateChildScope(stepsCount, span, name);
        }

        public static void Increase()
        {
            _threadProgressReporter.Value.IncreaseMostNestedScope();
        }

        public static void Complete()
        {
            _threadProgressReporter.Value.CompleteMostNestedScope();
        }

        public static void Subscribe(IProgressObserver progressObserver)
        {
            _threadProgressReporter.Value.Subscribe(progressObserver);
        }
    }
}
