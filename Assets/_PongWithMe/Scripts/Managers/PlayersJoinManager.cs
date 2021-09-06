using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class PlayersJoinManager : MonoBehaviour
    {
        private PlayersManager _playersManager = null;
        
        private List<PongInput> _inputList = new List<PongInput>();
        private List<PlayerPaddle> _playerPaddle = new List<PlayerPaddle>();
        private int _numberOfPlayersJoined = 0;
        
        public void Initialize(PlayersManager playersManager, int numberOfPlayers = 4)
        {
            _playersManager = playersManager;
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
            var player = new PlayerPaddle(input, _numberOfPlayersJoined, direction);
            _playerPaddle.Add(player);
            _numberOfPlayersJoined++;
            _playersManager.AddPlayer(player);
        }
    }
}