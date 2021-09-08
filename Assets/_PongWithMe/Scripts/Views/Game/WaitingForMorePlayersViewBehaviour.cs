using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class WaitingForMorePlayersViewBehaviour : MonoBehaviour
    {
        private const string TITLE = "Waiting for more players to join...";
        
        [SerializeField] private float _secondsBeforeStart = 15f;
        [SerializeField] private TMP_Text _title = null;

        private WaitingForMorePlayersViewModel _viewModel = null;
        private IStateManager _stateManager = null;

        public void Initialize(Action action, NetworkManager networkManager, IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            _viewModel = new WaitingForMorePlayersViewModel(networkManager, _secondsBeforeStart);
            _viewModel.OnElapsedTimeChanged += HandleOnElapsedTimeChanged;
            _viewModel.OnTimerEnded += action;
        }

        public void HideView()
        {
            transform.DOScale(Vector3.zero, 0.25f);
        }
        
        private void HandleOnElapsedTimeChanged(float elapsedTime)
        {
            _title.text = $"{(int) elapsedTime} \n {TITLE}";
        }

        private void Update()
        {
            if (_stateManager.GetState() == State.PlayerJoining)
            {
                _viewModel.Update(Time.deltaTime);
            }
        }
    }

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