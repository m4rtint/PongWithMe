using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkPlayerJoinManager : MonoBehaviour
    {
        private IPlayersManager _playersManager = null;
        
        private List<PongInput> _inputList = new List<PongInput>();

        private List<Direction> _takenDirection = new List<Direction>();
        private List<PongInput> _takenInput = new List<PongInput>();

        private int _numberOfPlayersJoined = 0;


        public void Initialize(IPlayersManager playersManager, int numberOfPlayers = 4)
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
                PhotonView.Get(this).RPC("AddPlayer", RpcTarget.All, (int) direction);
                _ownPaddle.Input = input;
            }
        }

        [PunRPC]
        private void AddPlayer(int direction)
        {
            _ownPaddle = new PlayerPaddle(_numberOfPlayersJoined, (Direction) direction);
            _numberOfPlayersJoined++;
            SetManagers(_ownPaddle);
        }

        private void SetManagers(IPaddle player)
        {
            _playersManager.AddPlayer(player);
            // TODO
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