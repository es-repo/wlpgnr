using System;

namespace WallpaperGenerator.Utilities
{
    public sealed class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onComplete;

        public Observer(Action<T> onNext = null, Action onComplete = null, Action<Exception> onError = null)
        {
            _onNext = onNext;
            _onComplete = onComplete;
            _onError = onError;
        }

        void IObserver<T>.OnNext(T value)
        {
            if (_onNext != null)
                _onNext(value);
        }

        void IObserver<T>.OnError(Exception error)
        {
            if (_onError != null)
                _onError(error);
        }

        void IObserver<T>.OnCompleted()
        {
            if (_onComplete != null)
                _onComplete();
        }
    }
}
