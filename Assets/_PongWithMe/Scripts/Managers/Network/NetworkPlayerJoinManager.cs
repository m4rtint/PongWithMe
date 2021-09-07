using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkPlayerJoinManager : MonoBehaviour
    {
        private NetworkPlayersManager _playersManager = null;
        
        private List<PongInput> _inputList = new List<PongInput>();

        private List<Direction> _takenDirection = new List<Direction>();
        private List<PongInput> _takenInput = new List<PongInput>();

        private int _numberOfPlayersJoined = 0;


        public void Initialize(NetworkPlayersManager playersManager, int numberOfPlayers = 4)
        {
            _playersManager = playersManager;
            SetupInputList(numberOfPlayers);
        }
        
        private void SetupInputList(int numberOfPlayers)
        {
            _inputList.Clear();
            for (int index = 0; index < numberOfPlayers; index++)
            {
                _inputList.Add(new PongInput(index));
            }
        }
        
        private void Update()
        {
            foreach (var input in _inputList)
            {
                if (input.IsPressingDown())
                {
                    InitializePlayerPaddle(input, Direction.Bottom);
                }
                
                if (input.IsPressingUp())
                {
                    InitializePlayerPaddle(input, Direction.Top);
                }
                
                if (input.IsPressingLeft())
                {
                    InitializePlayerPaddle(input, Direction.Left);
                }
                
                if (input.IsPressingRight())
                {
                    InitializePlayerPaddle(input, Direction.Right);
                }
            }
        }

        private PlayerPaddle _ownPaddle = null;
        
        private void InitializePlayerPaddle(PongInput input, Direction direction)
        {
            if (CanAddPlayer(input, direction))
            {
                var photon = PhotonView.Get(this);
                if (photon.IsMine)
                {
                    _ownPaddle = new PlayerPaddle(input, _numberOfPlayersJoined, direction);
                    _numberOfPlayersJoined++;
                    var paddlePhotonView = _playersManager.AddPlayer(_ownPaddle);
                    paddlePhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
                else
                {
                    photon.RPC("AddOtherPlayer", RpcTarget.Others, (int) direction);
                }
            }
        }

        [PunRPC]
        private void AddOtherPlayer(int direction)
        {
            _ownPaddle = new PlayerPaddle(_numberOfPlayersJoined, (Direction) direction);
            _numberOfPlayersJoined++;
            _playersManager.AddPlayer(_ownPaddle);
        }

        private bool CanAddPlayer(PongInput input, Direction direction)
        {
            if (_takenInput.Contains(input) || _takenInput.Count > 0)
            {
                return false;
            }

            if (_takenDirection.Contains(direction))
            {
                return false;
            }

            _takenInput.Add(input);
            _takenDirection.Add(direction);
            return true;
        }
    }

}