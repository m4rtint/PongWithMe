using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(TMP_Text))]
    public class CountDownViewBehaviour : MonoBehaviour
    {
        private const float COUNTDOWN_ANIMATION_DURATION = 1f;
        private const float COUNTDOWN_START_DELAY = 1f;
        private IStateManager _stateManager = null;
        private TMP_Text _countDownLabel = null;
        
        private void Awake()
        {
            _countDownLabel = GetComponent<TMP_Text>();
            _stateManager = StateManager.Instance;
            _stateManager.OnStateChanged += HandleStateChange;
        }
        
        private void Start()
        {            
            _countDownLabel.transform.localScale = Vector3.zero;
        }

        private void StartCountingDown()
        {
            var sequence = DOTween.Sequence();
            sequence.SetDelay(COUNTDOWN_START_DELAY);
            sequence.AppendCallback(() => SetLabelReadyToShow(3));
            sequence.Append(ShowCountDownLabel());
            sequence.AppendCallback(() => SetLabelReadyToShow(2));
            sequence.Append(ShowCountDownLabel());
            sequence.AppendCallback(() => SetLabelReadyToShow(1));
            sequence.Append(ShowCountDownLabel());
            sequence.AppendCallback(() => SetLabelReadyToShow(0));
            sequence.OnComplete(() =>
            {
                _stateManager.SetState(State.StartGame);
            });
        }

        private Tween ShowCountDownLabel()
        {
            return _countDownLabel.transform.DOScale(Vector3.one, COUNTDOWN_ANIMATION_DURATION).SetEase(Ease.OutQuint);
        }

        private void SetLabelReadyToShow(int value)
        {
            _countDownLabel.transform.localScale = Vector3.zero;
            SetLabelValue(value);
        }

        private void SetLabelValue(int value)
        {
            _countDownLabel.text = value.ToString();
        }

        private void HandleStateChange(State state)
        {
            if (state == State.PreGame)
            {
                StartCountingDown();
            }
        }
    }
}