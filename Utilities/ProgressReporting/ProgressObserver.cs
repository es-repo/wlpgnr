using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public class ProgressObserver : Observer<ProgressReportScope>, IProgressObserver
    {
        public ProgressObserver(Action<ProgressReportScope> onNext = null, Action onComplete = null)
            : this (onNext, null, onComplete)
        {
        }

        public ProgressObserver(Action<ProgressReportScope> onNext, TimeSpan? onNextFrequency, Action onComplete = null)
            : base(onNext, onNextFrequency, onComplete)
        {
        }
    }
}