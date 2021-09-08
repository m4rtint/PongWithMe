using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkPlayerJoinManager : MonoBehaviour
    {
        private NetworkPlayersManager _networkPlayersManager = null;
        private GoalsManager _goalsManager = null;
        
        private List<PongInput> _inputList = new List<PongInput>();

        private List<Direction> _takenDirection = new List<Direction>();
        private List<PongInput> _takenInput = new List<PongInput>();

        private int _numberOfPlayersJoined = 0;


        public void Initialize(
            NetworkPlayersManager playersManager, 
            GoalsManager goalsManager, 
            int numberOfPlayers = 4)
        {
            _goalsManager = goalsManager;
            _networkPlayersManager = playersManager;
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
        
        private void InitializePlayerPaddle(PongInput input, Direction direction)
        {
            if (CanAddPlayer(input, direction))
            {
                var ownPaddle = new PlayerPaddle(input, _numberOfPlayersJoined, direction);
                _numberOfPlayersJoined++;
                _networkPlayersManager.AddPlayer(ownPaddle, true);
                _goalsManager.Set(ownPaddle);
                PhotonView.Get(this).RPC("AddOtherPlayer", RpcTarget.OthersBuffered, (int) direction);
            }
        }

        [PunRPC]
        private void AddOtherPlayer(int direction)
        {
            var otherPaddle = new PlayerPaddle(_numberOfPlayersJoined, (Direction) direction);
            _numberOfPlayersJoined++;
            _networkPlayersManager.AddPlayer(otherPaddle);
            _goalsManager.Set(otherPaddle);
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