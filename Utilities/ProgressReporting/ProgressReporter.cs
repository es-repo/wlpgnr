using System.Runtime.CompilerServices;
using System.Threading;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public static class ProgressReporter
    {
        private readonly static ThreadLocal<ProgressReportScope> _scopePerThread = new ThreadLocal<ProgressReportScope>();

        public static ProgressReportScope CreateScope(double childScopeSpan = 1, [CallerMemberName] string name = "")
        {
            bool isFirstScope = _scopePerThread.Value == null;
            ProgressReportScope scope = _scopePerThread.Value ?? new ProgressReportScope(name);
            if (isFirstScope)
            {
                _scopePerThread.Value = scope;
            }
            else
            {
                while (scope.ChildScope != null) scope = scope.ChildScope;
                scope = scope.CreateChildScope(childScopeSpan, name);
            }

            return scope;
        }
    }
}
