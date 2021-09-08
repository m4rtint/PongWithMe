using System;

namespace PongWithMe
{
    public class WaitingForMorePlayersViewModel
    {
        private readonly INetworkManager _networkManager;
        private readonly float SecondsBeforeStart;
        private float _elapsedTime = 0;

        public event Action<float> OnElapsedTimeChanged;

        public event Action OnTimerEnded;

        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnElapsedTimeChanged?.Invoke(value);
            }
        }

        public WaitingForMorePlayersViewModel(INetworkManager networkManager, float secondBeforeStart)
        {
            _networkManager = networkManager;
            _networkManager.OnPlayerEntered += ResetTimer;
            SecondsBeforeStart = secondBeforeStart;
            _elapsedTime = secondBeforeStart;
        }

        private void ResetTimer()
        {
            _elapsedTime = SecondsBeforeStart;
        }

        public void Update(float deltaTime)
        {
            ElapsedTime -= deltaTime;
            if (ElapsedTime < 0)
            {
                OnTimerEnded?.Invoke();
            }
        }
    }
}