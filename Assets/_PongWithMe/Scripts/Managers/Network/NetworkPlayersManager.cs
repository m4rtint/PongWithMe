using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public partial class NetworkPlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour _topPaddle = null;
        [SerializeField] private PaddleBehaviour _leftPaddle = null;
        [SerializeField] private PaddleBehaviour _bottomPaddle = null;
        [SerializeField] private PaddleBehaviour _rightPaddle = null;
        
        private IPlayerLives _playerLives = null;
        
        private List<IPaddle> _players = new List<IPaddle>();
        private List<PaddleBehaviour> _paddleBehaviours = new List<PaddleBehaviour>();
        
        public List<IPaddle> Players => _players;
        

        public void Initialize(IPlayerLives lives)
        {
            _playerLives = lives;
            _playerLives.OnBrickBreak += HandleOnBrickBreak;
        }
        
        public void AddPlayer(IPaddle player, bool forceTakeover = false)
        {
            _players.Add(player);
            var paddleBehaviour = GetPaddleFrom(player.PaddleDirection);
            paddleBehaviour.Initialize(player);
            if (forceTakeover)
            {
                PhotonView.Get(paddleBehaviour).TransferOwnership(PhotonNetwork.LocalPlayer);
            }
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
        
        private void HandleOnBrickBreak(int brickOwner, int score)
        {
            if (score > 0)
            {
                return;
            }

            _players.First(player => player.PlayerNumber == brickOwner).IsActive = false;
        }
    }
        
    public partial class NetworkPlayersManager : IWinningPlayer
    {
        public IPaddle GetWinningPlayer()
        {
            return null;
        }
    }
    
    public partial class NetworkPlayersManager : IPaddleRotationMutator
    {
        public Tween RotatePaddles(bool isClockwiseDirection, float rotationDuration = 1f)
        {
            return null;
        }
    }
}