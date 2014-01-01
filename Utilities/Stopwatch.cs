using System;

namespace WallpaperGenerator.Utilities
{
    public class Stopwatch
    {
        private DateTime _startTime;

        public void Start()
        {
            _startTime = DateTime.Now;
        }

        public TimeSpan Stop()
        {
            return DateTime.Now - _startTime;
        }
    }
}
