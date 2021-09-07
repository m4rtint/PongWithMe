using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public partial class NetworkPlayersManager : MonoBehaviour, IPlayersManager
    {
        private const string PADDLE_PREFAB = "NetworkedPaddle";

        private IPlayerLives _playerLives = null;
        
        private List<IPaddle> _players = new List<IPaddle>();
        private List<PaddleBehaviour> _paddleBehaviours = new List<PaddleBehaviour>();
        
        public List<IPaddle> Players => _players;
        

        public void Initialize(IPlayerLives lives)
        {
            _playerLives = lives;
            _playerLives.OnBrickBreak += HandleOnBrickBreak;
        }
        
        public void AddPlayer(IPaddle player)
        {
            _players.Add(player);
            var paddleBehaviour = GetPaddleFor(player.PaddleDirection);
            paddleBehaviour.Initialize(player);
            player.IsActive = true;
        }

        private PaddleBehaviour GetPaddleFor(Direction direction)
        {
            var paddleGameObject = PhotonNetwork.Instantiate(PADDLE_PREFAB, Vector3.zero, Quaternion.identity);
            if (paddleGameObject.TryGetComponent<PaddleBehaviour>(out var paddle))
            {
                paddle.transform.SetParent(transform);
                _paddleBehaviours.Add(paddle);
                return paddle;
            }

            return null;
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