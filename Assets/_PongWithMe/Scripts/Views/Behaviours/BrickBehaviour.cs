using DG.Tweening;
using Shapes;
using UnityEngine;

namespace PongWithMe
{
    public class BrickBehaviour : MonoBehaviour
    {
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
            transform.DOScale(Vector3.zero, 0.5f)
                .SetUpdate(true)
                .SetEase(Ease.InBack).OnComplete(() =>
                {
                    _shape.Color = color;
                    transform.DOScale(Vector3.one, 0.5f)
                        .SetUpdate(true)
                        .SetEase(Ease.OutBack);
                });
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

