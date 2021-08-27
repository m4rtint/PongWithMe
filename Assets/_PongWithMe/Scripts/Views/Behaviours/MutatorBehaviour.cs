using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class MutatorBehaviour : MonoBehaviour
    {
        private const float ANIMATE_MUTATOR_DURATION = 0.5F;
        [SerializeField] private float _countDownToAppear = 50f;
        
        private MutatorManager _mutatorManager = null;
        private IStateManager _stateManager = null;
        
        private bool _canActivate = false;
        private bool _isShown = false;
        private float _elapsedTime = 0;
        
        public void Initialize(MutatorManager mutatorManager, IStateManager stateManager = null)
        {
            _mutatorManager = mutatorManager;
            _elapsedTime = _countDownToAppear;
            transform.localScale = Vector3.zero;
            _stateManager = stateManager ?? StateManager.Instance;
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
            transform.DOScale(Vector3.one, ANIMATE_MUTATOR_DURATION).SetEase(Ease.OutBack);
        }

        private void HideMutatorOnBoard()
        {
            _isShown = false;
            transform.DOScale(Vector3.zero, ANIMATE_MUTATOR_DURATION).SetEase(Ease.InBack);
        }
        
        private void ActivateMutator()
        {
            _stateManager.SetState(State.Animating);
            _mutatorManager.PickMutatorToActivate();
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
