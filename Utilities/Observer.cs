using System;

namespace WallpaperGenerator.Utilities
{
    public class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onComplete;

        private readonly TimeSpan _onNextFrequency;
        private readonly bool _onNextFrequencyIsZero;
        private DateTime _lastOnNextDate;

        public Observer(Action<T> onNext = null, Action onComplete = null, Action<Exception> onError = null)
            : this (onNext, null, onComplete, onError)
        {
        }

        public Observer(Action<T> onNext, TimeSpan? onNextFrequency, Action onComplete = null, Action<Exception> onError = null)
        {
            _onNext = onNext;
            _onComplete = onComplete;
            _onError = onError;
            _onNextFrequency = onNextFrequency != null ? onNextFrequency.Value : TimeSpan.Zero;
            _onNextFrequencyIsZero = _onNextFrequency == TimeSpan.Zero;
            _lastOnNextDate = DateTime.UtcNow;
        }

        void IObserver<T>.OnNext(T value)
        {
            if (_onNext != null)
            {
                if (_onNextFrequencyIsZero || (DateTime.UtcNow - _lastOnNextDate) > _onNextFrequency)
                {
                    _onNext(value);
                    _lastOnNextDate = DateTime.UtcNow;
                }
            }
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
