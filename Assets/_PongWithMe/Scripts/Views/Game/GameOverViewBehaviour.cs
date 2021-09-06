using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class GameOverViewBehaviour : MonoBehaviour
    {
        private const float EXPAND_PANEL_ANIMATION_DURATION = 0.5F;        
        private IStateManager _stateManager = null;
        private IWinningPlayer _winningPlayer = null;

        [SerializeField] private TMP_Text _gameOverDescriptionLabel = null;
        
        
        public void Initialize(IWinningPlayer winningPlayer, IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            _stateManager.OnStateChanged += HandleOnStateChanged;
            _winningPlayer = winningPlayer;
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.GameOver)
            {
                SetWinningPlayerText();
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

        private void SetWinningPlayerText()
        {
            var paddle = _winningPlayer.GetWinningPlayer();
            if (paddle != null)
            {
                _gameOverDescriptionLabel.text = "Player who won is " + paddle.PaddleDirection;
                _gameOverDescriptionLabel.color = paddle.PlayerColor;
            }
            else
            {
                _gameOverDescriptionLabel.text = "Thank you for playing";

            }
        }

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }
    }
}