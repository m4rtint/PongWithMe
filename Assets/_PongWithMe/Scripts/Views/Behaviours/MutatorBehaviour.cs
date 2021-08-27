using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class MutatorBehaviour : MonoBehaviour
    {
        [SerializeField] private float _countDownToAppear = 50f;
        
        private MutatorManager _mutatorManager = null;
        
        private bool _canActivate = false;
        private bool _isShown = false;
        private float _elapsedTime = 0;
        
        public void Initialize(MutatorManager mutatorManager)
        {
            _mutatorManager = mutatorManager;
            _elapsedTime = _countDownToAppear;
            transform.localScale = Vector3.zero;
        }

        [Button]
        public void ActivateMutator()
        {
            StateManager.Instance.SetState(State.Animating);
            _mutatorManager.PickMutatorToActivate();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<BallBehaviour>(out _) && _canActivate)
            {
                _canActivate = false;
                StartCountDown();
            }
        }

        private void FixedUpdate()
        {
            _elapsedTime -= Time.fixedDeltaTime;
            if (_elapsedTime < 0 && !_isShown)
            {
                ShowMutatorOnBoard();
                _elapsedTime = _countDownToAppear;
            }
        }

        private void ShowMutatorOnBoard()
        {
            _canActivate = true;
            _isShown = true;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void HideMutatorOnBoard()
        {
            _isShown = false;
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }

        private void StartCountDown()
        {
            // Placeholder as to how to inform the player mutator about to happen
            var duration = 0.1f;
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
                ActivateMutator();
                HideMutatorOnBoard();
            });
        }
    }
}
