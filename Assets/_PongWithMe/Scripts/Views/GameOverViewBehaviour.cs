using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class GameOverViewBehaviour : MonoBehaviour
    {
        private const float EXPAND_PANEL_ANIMATION_DURATION = 0.5F;
        private IStateManager _stateManager = null;

        public void Initialize(IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            _stateManager.OnStateChanged += HandleOnStateChanged;
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.GameOver)
            {
                transform.DOScale(Vector3.one, EXPAND_PANEL_ANIMATION_DURATION)
                    .SetEase(Ease.InOutSine)
                    .SetUpdate(true);
            }

            if (state == State.Play)
            {
                transform.DOScale(Vector3.zero, EXPAND_PANEL_ANIMATION_DURATION)
                    .SetEase(Ease.InOutSine)
                    .SetUpdate(true);
            }
        }

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }
    }
}