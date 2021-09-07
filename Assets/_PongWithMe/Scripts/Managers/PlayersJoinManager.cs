using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class PlayersJoinManager : MonoBehaviour
    {
        private PlayersManager _playersManager = null;
        private GoalsManager _goalsManager = null;
        private IBall _ball = null;
        private IScoreView _scoreView = null;
        
        private List<PongInput> _inputList = new List<PongInput>();
        private int _numberOfPlayersJoined = 0;
        
        private List<PongInput> _takenInput = new List<PongInput>();
        private List<Direction> _takenDirection = new List<Direction>();
        
        public void Initialize(
            PlayersManager playersManager, 
            GoalsManager goalsManager,
            IBall ball,
            IScoreView scoreView,
            int numberOfPlayers = 4)
        {
            _playersManager = playersManager;
            _goalsManager = goalsManager;
            _ball = ball;
            _scoreView = scoreView;
            SetupInputList(numberOfPlayers);
        }

        public void FillInPlayerSlotsWithAI()
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
                    SetManagers(aiPaddle);
                }
            }
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
                var player = new PlayerPaddle(input, _numberOfPlayersJoined, direction);
                _numberOfPlayersJoined++;
                SetManagers(player);
            }
        }

        private void SetManagers(IPaddle player)
        {
            _playersManager.AddPlayer(player);
            _goalsManager.Set(player);
            _scoreView.AddPlayer(player);
        }

        private bool CanAddPlayer(PongInput input, Direction direction)
        {
            if (_takenInput.Contains(input))
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