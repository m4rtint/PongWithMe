using UnityEngine;

namespace PongWithMe
{
    public class PaddleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 0.2f;

        private IPaddle _player = null;
        private PaddleMovementBehaviour _movementBehaviour = null;
        
        public void Initialize(IPaddle playerPaddle)
        {
            _player = playerPaddle;
            _movementBehaviour.Initialize(playerPaddle.Input, playerPaddle.PaddleDirection, _speed);
            if (playerPaddle is AIPaddle paddle)
            {
                var ai = gameObject.AddComponent<AIPaddleBehaviour>();
                ai.Initialize(paddle);
            }
        }

        private void Awake()
        {
            _movementBehaviour = GetComponent<PaddleMovementBehaviour>();
        }
    }
}

