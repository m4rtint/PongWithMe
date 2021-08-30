using DG.Tweening;
using PerigonGames;
using UnityEngine;

namespace PongWithMe
{
    public class SplattersBehaviour : MonoBehaviour
    {
        private const float SHOW_SPLATTER_ANIMATION_DURATION = 0.5F;
        private const float HIDE_SPLATTER_ANIMATION_DURATION = 2.0F;
        
        [SerializeField] private SpriteRenderer _render1 = null;
        [SerializeField] private SpriteRenderer _render2 = null;
        [SerializeField] private SpriteRenderer _render3 = null;

        [SerializeField] private float _timeToLives = 3f;
        
        private Splatter _splatter = null;
        public Splatter Splatters => _splatter;

        public void Initialize(Splatter splatter = null)
        {
            _splatter = splatter ?? new Splatter(_timeToLives);
            _splatter.OnActivateChanged += HandleOnActivate;
        }

        private void HandleOnActivate(bool state)
        {
            if (state)
            {
                ActivateSplatters();
            }
            else
            {
                DeactivateSplatters();
            }
        }

        private void ActivateSplatters()
        {
            ResetColor();
            ResetScale();
            var random = new RandomUtility();
            var sequence = DOTween.Sequence();
            sequence.Insert((float)random.NextDouble(), ShowSplatter(_render1)).SetUpdate(true);
            sequence.Insert((float)random.NextDouble(), ShowSplatter(_render2));
            sequence.Insert((float)random.NextDouble(), ShowSplatter(_render3));
            sequence.OnComplete(() =>
            {
                StateManager.Instance.SetState(State.Play);
            });
        }

        private void ResetColor()
        {
            _render1.color = Color.white;
            _render2.color = Color.white;
            _render3.color = Color.white;
        }

        private void ResetScale()
        {
            _render1.transform.localScale = Vector3.zero;
            _render2.transform.localScale = Vector3.zero;
            _render3.transform.localScale = Vector3.zero;
        }

        private Tween ShowSplatter(SpriteRenderer renderer)
        {
            return renderer.transform.DOScale(Vector3.one, SHOW_SPLATTER_ANIMATION_DURATION).SetEase(Ease.OutBack);
        }

        private void DeactivateSplatters()
        {
            var random = new RandomUtility();

            _render1.DOColor(Color.clear, HIDE_SPLATTER_ANIMATION_DURATION).SetDelay((float)random.NextDouble());
            _render2.DOColor(Color.clear, HIDE_SPLATTER_ANIMATION_DURATION).SetDelay((float)random.NextDouble());
            _render3.DOColor(Color.clear, HIDE_SPLATTER_ANIMATION_DURATION).SetDelay((float)random.NextDouble());
        }

        private void Awake()
        {
            ResetScale();
        }

        private void Update()
        {
            _splatter.Update(Time.deltaTime);
        }
    }
}