using System;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public interface IProgressObservable : IObservable<ProgressReportScope>
    {
    }
}
