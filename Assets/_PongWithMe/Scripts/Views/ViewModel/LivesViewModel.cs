using System;
using System.Collections.Generic;

namespace PongWithMe
{
    public class LivesViewModel
    {
        private IPlayerLives _playersLives = null;
        private List<IPaddle> _players = null;

        public event Action<Direction, int> OnScoreUpdate;
        public event Action<List<IPaddle>> OnPlayerDirectionChanged;

        public LivesViewModel(IPlayerLives lives, List<IPaddle> players)
        {
            _players = players;
            _playersLives = lives;
            _playersLives.OnBrickBreak += HandleOnBrickBreak;
            foreach (var player in players)
            {
                player.OnDirectionChanged += HandlePlayerDirectionChanged;
            }
        }

        public int NumberOfLivesFor(int owner)
        {
            return _playersLives.GetPlayerLives(owner);
        }

        private void HandleOnBrickBreak(int brickOwnerNumber, int numberOfLives)
        {
            var paddle = GetPlayerPaddle(brickOwnerNumber);
            OnScoreUpdate?.Invoke(paddle.PaddleDirection, numberOfLives);
        }

        private void HandlePlayerDirectionChanged(Direction direction)
        {
            OnPlayerDirectionChanged?.Invoke(_players);
        }

        private IPaddle GetPlayerPaddle(int brickOwnerNumber)
        {
            foreach (var player in _players)
            {
                if (player.PlayerNumber == brickOwnerNumber)
                {
                    return player;
                }
            }

            return null;
        }
    }
}