using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public class ProgressObserver : Observer<double>, IProgressObserver
    {
        public ProgressObserver(Action<double> onNext = null, Action onComplete = null)
            : base(onNext, onComplete)
        {
        }
    }
}