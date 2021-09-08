using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkPlayerJoinManager : MonoBehaviour
    {
        private NetworkPlayersManager _networkPlayersManager = null;
        private GoalsManager _goalsManager = null;
        private IBall _ball = null;
        
        private List<PongInput> _inputList = new List<PongInput>();

        private List<Direction> _takenDirection = new List<Direction>();
        private List<PongInput> _takenInput = new List<PongInput>();

        private int _numberOfPlayersJoined = 0;


        public void Initialize(
            NetworkPlayersManager playersManager, 
            GoalsManager goalsManager,
            IBall ball,
            int numberOfPlayers = 4)
        {
            _goalsManager = goalsManager;
            _networkPlayersManager = playersManager;
            _ball = ball;
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
        
        public void CompletePlayerJoiningSession()
        {
            FillInPlayerSlotsWithAI();
        }
        
        private void FillInPlayerSlotsWithAI()
        {
            var directions = new[] { Direction.Bottom, Direction.Left, Direction.Right, Direction.Top };
            foreach (var direction in directions)
            {
                var isDirectionOpen = !_takenDirection.Contains(direction);
                if (isDirectionOpen)
                {
                    var aiInput = new AIInput();
                    var aiPaddle = new AIPaddle(aiInput, _numberOfPlayersJoined, direction, _ball);
                    _numberOfPlayersJoined++;
                    
                    _networkPlayersManager.AddPlayer(aiPaddle);
                    _goalsManager.Set(aiPaddle);
                }
            }
        }
        
        private void InitializePlayerPaddle(PongInput input, Direction direction)
        {
            if (CanAddPlayer(input, direction))
            {
                var ownPaddle = new PlayerPaddle(input, _numberOfPlayersJoined, direction);
                _numberOfPlayersJoined++;
                PhotonView.Get(this).RPC("AddOtherPlayer", RpcTarget.OthersBuffered, (int) direction);
                
                _networkPlayersManager.AddPlayer(ownPaddle, true);
                _goalsManager.Set(ownPaddle);
            }
        }

        [PunRPC]
        private void AddOtherPlayer(int direction)
        {
            _takenDirection.Add((Direction) direction);
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