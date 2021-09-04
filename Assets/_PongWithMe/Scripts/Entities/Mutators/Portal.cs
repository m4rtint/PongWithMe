using System;

namespace PongWithMe
{
    public class Portal
    {
        private readonly int _index = 0;

        private bool _isActive = false;
        private bool _canTeleport = true;

        public event Action<IBall, int> OnBallEnteredPortal;
        public event Action<bool> OnIsActiveChanged;

        public bool CanTeleport
        {
            set => _canTeleport = value;
        }

        public bool IsActive
        {
            set
            {
                _isActive = value;
                OnIsActiveChanged?.Invoke(value);
            }
        }

        public Portal(int index)
        {
            _index = index;
        }

        public void BallEnteredPortal(IBall ball)
        {
            if (_canTeleport && _isActive)
            {
                OnBallEnteredPortal?.Invoke(ball, _index);
            }
        }

        public void CleanUp()
        {
            IsActive = false;
            _canTeleport = true;
        }
    }
}