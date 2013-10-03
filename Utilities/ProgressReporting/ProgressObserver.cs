using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public class ProgressObserver : Observer<ProgressReportScope>, IProgressObserver
    {
        public ProgressObserver(Action<ProgressReportScope> onNext = null, Action onComplete = null)
            : base(onNext, onComplete)
        {
        }
    }
}