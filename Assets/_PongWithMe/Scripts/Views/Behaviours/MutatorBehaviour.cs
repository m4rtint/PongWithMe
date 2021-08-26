using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class MutatorBehaviour : MonoBehaviour
    {
        private bool hasActivated = false;
        private MutatorManager _mutatorManager = null;
        public void Initialize(MutatorManager mutatorManager)
        {
            _mutatorManager = mutatorManager;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<BallBehaviour>(out _) && !hasActivated)
            {
                hasActivated = true;
                StartCountDown();
            }
        }

        private void StartCountDown()
        {
            var duration = 0.25f;
            var large = Vector3.one * 1.3f;
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(large, duration));
            seq.Append(transform.DOScale(Vector3.one, duration));
            seq.Append(transform.DOScale(large, duration));
            seq.Append(transform.DOScale(Vector3.one, duration));
            seq.Append(transform.DOScale(large, duration));
            seq.Append(transform.DOScale(Vector3.one, duration));
            seq.OnComplete(() =>
            {
                _mutatorManager.PickMutatorToActivate();
            });
        }
    }
}
