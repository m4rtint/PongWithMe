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
        private IBrickPopEffect _popEffectPool = null;
        
        public void Initialize(Brick brick, IBrickPopEffect popEffectPool = null)
        {
            _popEffectPool = popEffectPool ?? BrickPopEffectsPool.Instance;
            _brick = brick;
            _brick.OnBrickColorSet += HandleOnBrickColorSet;
            _brick.OnBrickPositionSet += HandleOnBrickPositionSet;
            _brick.OnBrickIsActiveSet += HandleBrickIsActiveSet;
            transform.position = _brick.Position;
            transform.localScale = Vector3.zero;
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

        private void PlayBrickBreakParticleEffect(Color color)
        {
            var effect = _popEffectPool.GetEffect();
            effect.SetColor(color);
            effect.transform.position = transform.position;
            effect.gameObject.SetActive(true);
            effect.Play();
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
            PlayBrickBreakParticleEffect(brick.BrickColor);
            gameObject.SetActive(isActive);
        }
        #endregion
    }
}

