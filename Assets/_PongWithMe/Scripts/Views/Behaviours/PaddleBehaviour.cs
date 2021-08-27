using DG.Tweening;
using Shapes;
using UnityEngine;

namespace PongWithMe
{
    public class PaddleBehaviour : MonoBehaviour
    {
        private const float DEATH_ANIMATION_DURATION = 0.5F;
        
        [SerializeField]
        private float _speed = 0.2f;
        [SerializeField] 
        private Rectangle _rectangle = null;

        private IPaddle _player = null;
        private PaddleMovementBehaviour _movementBehaviour = null;

        public Color PaddleColor => _player.PlayerColor;
        
        public void Initialize(IPaddle playerPaddle)
        {
            _player = playerPaddle;
            _movementBehaviour.Initialize(playerPaddle.Input, playerPaddle.PaddleDirection, _speed);
            SetupAIIfNeeded(playerPaddle);
            SetupStyle();
            _player.OnIsActiveUpdated += HandleIsActiveUpdated;
            _player.OnDirectionChanged += HandleOnDirectionChanged;
        }

        private void SetupAIIfNeeded(IPaddle playerPaddle)
        {
            if (playerPaddle is AIPaddle paddle)
            {
                var ai = gameObject.AddComponent<AIPaddleBehaviour>();
                ai.Initialize(paddle);
            }
        }

        private void SetupStyle()
        {
            _rectangle.Color = _player.PlayerColor;
        }

        private void Awake()
        {
            _movementBehaviour = GetComponent<PaddleMovementBehaviour>();
        }

        private void OnDestroy()
        {
            _player.OnIsActiveUpdated -= HandleIsActiveUpdated;
        }

        #region Delegate

        private void HandleIsActiveUpdated(bool active)
        {
            if (!active)
            {
                transform.DOScale(Vector3.zero, DEATH_ANIMATION_DURATION)
                    .SetEase(Ease.InBack)
                    .SetUpdate(true);
            }
        }

        private void HandleOnDirectionChanged(Direction direction)
        {
            _movementBehaviour.Set(direction);
        }
        #endregion
    }
}
