using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class MutatorBehaviour : MonoBehaviour
    {
        private const float ANIMATE_MUTATOR_DURATION = 0.5F;
        
        [SerializeField] private float _countDownToAppear = 50f;
        
        private MutatorManager _mutatorManager = null;
        
        private bool _canActivate = false;
        private bool _isShown = false;
        private float _elapsedTime = 0;
        private Sequence sequence;
        
        public void Initialize(MutatorManager mutatorManager)
        {
            _mutatorManager = mutatorManager;
            _elapsedTime = _countDownToAppear;
            transform.localScale = Vector3.zero;
        }

        public void CleanUp()
        {
            sequence.Kill();
            _canActivate = false;
            _isShown = false;
            _elapsedTime = 0;
            transform.localScale = Vector3.zero;
        }

        public void Reset()
        {
            _canActivate = true;
            _isShown = false;
            _elapsedTime = _countDownToAppear;
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
            if (!_isShown)
            {
                _elapsedTime -= Time.fixedDeltaTime;
                if (_elapsedTime  < 0)
                {
                    ShowMutatorOnBoard();
                    _elapsedTime = _countDownToAppear;
                }
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
            _mutatorManager.PickMutatorToActivate();
        }

        [Button]
        private void StartCountDown()
        {
            // Placeholder as to how to inform the player mutator about to happen
            var duration = 0.1f;
            var large = Vector3.one * 1.3f;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(large, duration));
            sequence.Append(transform.DOScale(Vector3.one, duration));
            sequence.Append(transform.DOScale(large, duration));
            sequence.Append(transform.DOScale(Vector3.one, duration));
            sequence.Append(transform.DOScale(large, duration));
            sequence.Append(transform.DOScale(Vector3.one, duration));
            sequence.OnComplete(() =>
            {
                ActivateMutator();
                HideMutatorOnBoard();
            });
        }
    }
}
