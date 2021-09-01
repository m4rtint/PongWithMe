using System;

namespace PongWithMe
{
    public class Splatter
    {
        private readonly float _timeToLive = 0;
        private float _elapsedTime = 0;
        private bool _isActive = false;

        public event Action<bool> OnActivateChanged;

        private bool IsActive
        {
            set
            {
                _isActive = value;
                OnActivateChanged?.Invoke(value);
            }
        }

        public Splatter(float timeToLive)
        {
            _timeToLive = timeToLive;
        }

        public void ActivateSplatters()
        {
            _elapsedTime = _timeToLive;
            IsActive = true;
        }

        public void Reset()
        {
            _elapsedTime = _timeToLive;
            IsActive = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            _elapsedTime -= deltaTime;
            if (_elapsedTime < 0)
            {
                IsActive = false;
            }
        }
    }
}
