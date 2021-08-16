using UnityEngine;

namespace PongWithMe
{
    public class PaddleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 0.2f;

        private PlayerPaddle _player = null;
        private PaddleMovementBehaviour _movementBehaviour = null;
        
        public void Initialize(PlayerPaddle playerPaddle)
        {
            _player = playerPaddle;
            _movementBehaviour.Initialize(playerPaddle.PlayerInput, playerPaddle.PaddleDirection, _speed);
        }

        private void Awake()
        {
            _movementBehaviour = GetComponent<PaddleMovementBehaviour>();
        }
    }
}

