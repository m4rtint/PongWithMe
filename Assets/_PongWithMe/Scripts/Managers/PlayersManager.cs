using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace PongWithMe
{
    public interface IWinningPlayer
    {
        IPaddle GetWinningPlayer();
    }

    public interface IPaddleRotationMutator
    {
        Tween RotatePaddles(bool isClockwiseDirection, float rotationDuration = 2f);
    }
    
    public partial class PlayersManager : MonoBehaviour
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
            var player = new PlayerPaddle(input, 0, Direction.Left);
            _players.Add(player);
            _leftPaddle.Initialize(player);
            
            var input2 = new PongInput(1);  
            var player2 = new PlayerPaddle(input2, 1, Direction.Right);
            _players.Add(player2);
            _rightPaddle.Initialize(player2);
            
            var ai = new AIInput();
            var aiPaddle = new AIPaddle(ai, 2, Direction.Top, _ball);
            _players.Add(aiPaddle);
            _topPaddle.Initialize(aiPaddle);
            /*
            var ai2 = new AIInput();
            var aiPaddle2 = new AIPaddle(ai2, 2, Direction.Bottom, _ball);
            _players.Add(aiPaddle2);
            _bottomPaddle.Initialize(aiPaddle2);
            */
            
            var ai3 = new AIInput();
            var aiPaddle3 = new AIPaddle(ai3, 3, Direction.Bottom, _ball);
            _players.Add(aiPaddle3);
            _bottomPaddle.Initialize(aiPaddle3);
        }

        private void OnDestroy()
        {
            _playerLives.OnBrickBreak-= HandleOnBrickBreak;
        }
    }

    public partial class PlayersManager : IWinningPlayer
    {
        public IPaddle GetWinningPlayer()
        {
            var playersAlive = 0;
            foreach (var paddle in _players)
            {
                if (paddle.IsActive)
                {
                    playersAlive++;
                }
            }

            if (playersAlive > 1)
            {
                return null;
            }

            return _players.FirstOrDefault(paddle => paddle.IsActive);
        }
    }

    public partial class PlayersManager : IPaddleRotationMutator
    {
        public Tween RotatePaddles(bool isClockwiseDirection, float rotationDuration = 2f)
        {
            var rotateBy = isClockwiseDirection ? -90 : 90;
            var rotationEndValue = new Vector3(0, 0, rotateBy);
            return transform.DORotate(
                rotationEndValue,
                rotationDuration, 
                RotateMode.WorldAxisAdd)
                .SetUpdate(true);
        }
    }
}

