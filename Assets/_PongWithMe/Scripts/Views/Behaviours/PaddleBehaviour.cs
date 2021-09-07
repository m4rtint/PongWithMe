using DG.Tweening;
using Photon.Pun;
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

        private Vector3 _originalSpawn = Vector3.zero;

        public Color PaddleColor => _player.PlayerColor;
        public Direction PaddleDirection => _player.PaddleDirection;
        
        public void Initialize(IPaddle playerPaddle)
        {
            _player = playerPaddle;
            _movementBehaviour.Initialize(_player.Input, _player.PaddleDirection, speed: _speed);
            SetupAIIfNeeded(playerPaddle);
            SetupStyle();
            _player.OnIsActiveUpdated += HandleIsActiveUpdated;
            _player.OnDirectionChanged += HandleOnDirectionChanged;
        }

        public void Reset()
        {
            transform.position = _originalSpawn;
            _movementBehaviour.Set(_player.PaddleDirection);
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
            _originalSpawn = transform.localPosition;
            transform.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.OnIsActiveUpdated -= HandleIsActiveUpdated;
            }
        }

        #region Delegate

        private void HandleIsActiveUpdated(bool active)
        {
            var size = active ? Vector3.one : Vector3.zero;
            var ease = active ? Ease.OutBack : Ease.InBack;
            transform.DOScale(size, DEATH_ANIMATION_DURATION)
                    .SetEase(ease)
                    .SetUpdate(true).OnComplete(() =>
                    {
                        var photon = PhotonView.Get(this);
                        photon.RPC("ColorSet", RpcTarget.All);
                    });
        }

        private void HandleOnDirectionChanged(Direction direction)
        {
            _movementBehaviour.Set(direction);
        }
        #endregion
        
        #region RPC

        [PunRPC]
        private void ColorSet()
        {
            _rectangle.Color = _player.PlayerColor;
        }
        #endregion
    }
}

