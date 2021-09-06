using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
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
        
        // Dependencies
        private IBall _ball = null;        
        private IPlayerLives _playerLives = null;

        private List<IPaddle> _players = new List<IPaddle>();
        public List<IPaddle> Players => _players;

        public void Initialize(IBall ball, IPlayerLives lives)
        {
            _playerLives = lives;
            _playerLives.OnBrickBreak += HandleOnBrickBreak;
            _ball = ball;
            SetupPlayers();
        }

        public void CleanUp()
        {
            transform.localRotation = Quaternion.identity;
        }

        public void Reset()
        {
            _players[0].PaddleDirection = Direction.Left;
            _players[1].PaddleDirection = Direction.Right;
            _players[2].PaddleDirection = Direction.Top;
            _players[3].PaddleDirection = Direction.Bottom;
            
            _players.ForEach(player => player.Reset());
            
            _leftPaddle.Reset();
            _topPaddle.Reset();
            _rightPaddle.Reset();
            _bottomPaddle.Reset();
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
        private const float RIGHT_ANGLE = 90F;
        
        public Tween RotatePaddles(bool isClockwiseDirection, float rotationDuration = 1f)
        {
            var rotateBy = isClockwiseDirection ? -RIGHT_ANGLE : RIGHT_ANGLE;
            var rotationEndValue = Vector3.forward * rotateBy;
            return transform.DORotate(
                rotationEndValue,
                rotationDuration, 
                RotateMode.WorldAxisAdd)
                .SetUpdate(true);
        }
    }
}

