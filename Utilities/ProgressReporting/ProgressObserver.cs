using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public class ProgressObserver : Observer<double>
    {
        public ProgressObserver(Action<double> onNext = null, Action onComplete = null)
            : base(onNext, onComplete)
        {
        }
    }
}