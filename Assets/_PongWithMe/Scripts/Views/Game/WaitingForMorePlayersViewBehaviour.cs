using System;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class WaitingForMorePlayersViewBehaviour : MonoBehaviour, IPunObservable
    {
        private const string TITLE = "Waiting for more players to join...";
        
        [SerializeField] private float _secondsBeforeStart = 30f;
        [SerializeField] private TMP_Text _title = null;

        private WaitingForMorePlayersViewModel _viewModel = null;
        private IStateManager _stateManager = null;

        private Action OnTimerEndAction = null;

        public void Initialize(Action timerEndAction, NetworkManager networkManager, IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            OnTimerEndAction = timerEndAction;
            _viewModel = new WaitingForMorePlayersViewModel(networkManager, _secondsBeforeStart);
            _viewModel.OnElapsedTimeChanged += HandleOnElapsedTimeChanged;
            _viewModel.OnTimerEnded += OnTimerEndAction;
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

        private void OnDestroy()
        {
            if (_viewModel != null)
            {
                _viewModel.OnElapsedTimeChanged -= HandleOnElapsedTimeChanged;
                _viewModel.OnTimerEnded -= OnTimerEndAction; 
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (stream.IsWriting)
                {
                    stream.SendNext(_viewModel.ElapsedTime);
                }
            }

            if (stream.IsReading)
            {
                _viewModel.ElapsedTime = (float) stream.ReceiveNext();
            }
        }
    }
}