using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

    public interface IPlayersManager
    {
        void AddPlayer(IPaddle player);
    }
    
    public partial class PlayersManager : MonoBehaviour, IPlayersManager
    {
        [SerializeField] private PaddleBehaviour _topPaddle = null;
        [SerializeField] private PaddleBehaviour _leftPaddle = null;
        [SerializeField] private PaddleBehaviour _bottomPaddle = null;
        [SerializeField] private PaddleBehaviour _rightPaddle = null;
        
        // Dependencies
        private IPlayerLives _playerLives = null;

        private List<IPaddle> _players = new List<IPaddle>();
        public List<IPaddle> Players => _players;

        public void Initialize(IPlayerLives lives)
        {
            _playerLives = lives;
            _playerLives.OnBrickBreak += HandleOnBrickBreak;
        }

        public void CleanUp()
        {
            transform.localRotation = Quaternion.identity;
        }

        public void Reset()
        {
            _players.ForEach(player => player.Reset());
            
            _leftPaddle.Reset();
            _topPaddle.Reset();
            _rightPaddle.Reset();
            _bottomPaddle.Reset();
        }

        public void AddPlayer(IPaddle player)
        {
            _players.Add(player);
            var paddleBehaviour = GetPaddleFrom(player.PaddleDirection);
            paddleBehaviour.Initialize(player);
            player.IsActive = true;
        }

        private PaddleBehaviour GetPaddleFrom(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return _topPaddle;
                case Direction.Right:
                    return _rightPaddle;
                case Direction.Bottom:
                    return _bottomPaddle;
                case Direction.Left:
                    return _leftPaddle;
                default:
                    PanicHelper.Panic(new Exception("Direction passed in does not exist"));
                    return _topPaddle;
            }
        }

        private void OnDestroy()
        {
            _playerLives.OnBrickBreak-= HandleOnBrickBreak;
        }
        
        private void HandleOnBrickBreak(int brickOwner, int score)
        {
            if (score > 0)
            {
                return;
            }

            _players.First(player => player.PlayerNumber == brickOwner).IsActive = false;
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

