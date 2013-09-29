using System;

namespace WallpaperGenerator.Utilities
{
    public sealed class Disposable : IDisposable
    {
        private bool _isDisposed;
        private readonly Action _dispose;

        public Disposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            if (_isDisposed)
                throw new InvalidOperationException("Already is disposed.");

            _dispose();
            _isDisposed = true;
        }
    }
}
