using DG.Tweening;
using Shapes;
using UnityEngine;

namespace PongWithMe
{
    public class BrickBehaviour : MonoBehaviour
    {
        private const float BRICK_BREAK_ANIMATION_DURATION = 0.5F;
        [SerializeField] private Disc _shape = null;

        private Brick _brick = null;
        
        public void Initialize(Brick brick)
        {
            _brick = brick;
            _brick.OnBrickColorSet += HandleOnBrickColorSet;
            _brick.OnBrickPositionSet += HandleOnBrickPositionSet;
            _brick.OnBrickIsActiveSet += HandleBrickIsActiveSet;
            transform.position = _brick.Position;
        }

        private void OnDisable()
        {
            if (_brick != null)
            {
                _brick.OnBrickColorSet -= HandleOnBrickColorSet;
                _brick.OnBrickPositionSet -= HandleOnBrickPositionSet;
                _brick.OnBrickIsActiveSet -= HandleBrickIsActiveSet;
                _brick = null;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out BallBehaviour ball))
            {
                _brick.IsActive = false;
            }
        }

        #region Delegate

        private void HandleOnBrickColorSet(Brick brick, Color color)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.zero, BRICK_BREAK_ANIMATION_DURATION).SetEase(Ease.InBack)).SetUpdate(true);
            sequence.AppendCallback(() =>
            {
                _shape.Color = color;
            });
            sequence.Append(transform.DOScale(Vector3.one, BRICK_BREAK_ANIMATION_DURATION).SetEase(Ease.OutBack));
        }

        private void HandleOnBrickPositionSet(Brick brick, Vector3 position)
        {
            transform.position = position;
        }

        private void HandleBrickIsActiveSet(Brick brick, bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        #endregion
    }
}

