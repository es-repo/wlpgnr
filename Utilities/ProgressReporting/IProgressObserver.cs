using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public interface IProgressObserver : IObserver<double>
    {
    }
}