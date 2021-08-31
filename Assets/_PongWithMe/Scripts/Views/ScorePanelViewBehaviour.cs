using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class ScorePanelViewBehaviour : MonoBehaviour
    {
        private const float DELAY_SCORE_DISPLAY = 1.5F;
        private const float SHOW_HIDE_SCOREBOARD_DURATION = 1.0F;
        private const float SHOW_WIN_SQUARE_DELAY = 1.0F;
        private const float DELAY_HIDE_SCORE_DISPLAY = 2F;
        
        [SerializeField]
        private PlayerScoreViewBehaviour[] _playerScorePanels = null;

        private IWinningPlayer _winningPlayer = null;
        private IStateManager _stateManager = null;
        
        private Dictionary<IPaddle, PlayerScoreViewBehaviour> _dictionaryScores =
            new Dictionary<IPaddle, PlayerScoreViewBehaviour>();

        public void Initialize(
            List<IPaddle> players, 
            IWinningPlayer winningPlayer, 
            IStateManager stateManager = null, 
            int lives = 3)
        {
            _winningPlayer = winningPlayer;
            _stateManager = stateManager ?? StateManager.Instance;
            _stateManager.OnStateChanged += HandleOnStateChanged;
            
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var playerScorePanel = _playerScorePanels[i];
                playerScorePanel.Initialize(player.PlayerColor, lives);
                _dictionaryScores.Add(player, playerScorePanel);
            }

            transform.localScale = Vector3.zero;
        }
        
        private void PlayerWin(IPaddle paddle)
        {
            var sequence = DOTween.Sequence();
            sequence.SetDelay(DELAY_SCORE_DISPLAY);
            sequence.Append(transform.DOScale(Vector3.one, SHOW_HIDE_SCOREBOARD_DURATION)).SetUpdate(true);
            var scoreView = _dictionaryScores[paddle];
            sequence.Append(scoreView.WinNextSquare().SetDelay(SHOW_WIN_SQUARE_DELAY));
            sequence.Append(transform.DOScale(Vector3.zero, SHOW_HIDE_SCOREBOARD_DURATION).SetDelay(DELAY_HIDE_SCORE_DISPLAY));
            sequence.OnComplete(() =>
            {
                _stateManager.SetState(State.PreGame);
            });
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.ShowScore)
            {
                var player = _winningPlayer.GetWinningPlayer();
                PlayerWin(player);
            }
        }

        private void OnDestroy()
        {
            _stateManager.OnStateChanged -= HandleOnStateChanged;
        }
    }
}