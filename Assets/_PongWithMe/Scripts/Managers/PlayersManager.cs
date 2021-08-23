using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PongWithMe
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour _topPaddle = null;
        [SerializeField] private PaddleBehaviour _leftPaddle = null;
        [SerializeField] private PaddleBehaviour _bottomPaddle = null;
        [SerializeField] private PaddleBehaviour _rightPaddle = null;
        
        private IBall _ball = null;
        private List<IPaddle> _players = new List<IPaddle>();
        private IPlayerLives _playerLives = null;

        public List<IPaddle> Players => _players;

        public void Initialize(IBall ball, IPlayerLives lives)
        {
            _playerLives = lives;
            _playerLives.OnBrickBreak += HandleOnBrickBreak;
            _ball = ball;
            SetupPlayers();
        }

        private void HandleOnBrickBreak(int brickOwner, int score)
        {
            if (score > 0)
            {
                return;
            }

            _players.First(player => player.PlayerNumber == brickOwner).IsActive = false;
        }

        private void SetupPlayers()
        {
            var input = new PongInput(0);                
            var player = new PlayerPaddle(input, 0, Direction.Top);
            _players.Add(player);
            _topPaddle.Initialize(player);
            
            var ai = new AIInput();
            var aiPaddle = new AIPaddle(ai, 1, Direction.Left, _ball);
            _players.Add(aiPaddle);
            _leftPaddle.Initialize(aiPaddle);
            
            var ai2 = new AIInput();
            var aiPaddle2 = new AIPaddle(ai2, 2, Direction.Bottom, _ball);
            _players.Add(aiPaddle2);
            _bottomPaddle.Initialize(aiPaddle2);
            
            var ai3 = new AIInput();
            var aiPaddle3 = new AIPaddle(ai3, 3, Direction.Right, _ball);
            _players.Add(aiPaddle3);
            _rightPaddle.Initialize(aiPaddle3);
        }

        private void OnDestroy()
        {
            _playerLives.OnBrickBreak-= HandleOnBrickBreak;
        }
    }
}

