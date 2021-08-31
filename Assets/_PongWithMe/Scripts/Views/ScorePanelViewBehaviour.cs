using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class ScorePanelViewBehaviour : MonoBehaviour
    {
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
            transform.DOScale(Vector3.one, 1.0f).SetUpdate(true).OnComplete(() =>
            {
                var scoreView = _dictionaryScores[paddle];
                scoreView.WinNextSquare().SetUpdate(true).OnComplete(() =>
                {
                    transform.DOScale(Vector3.zero, 1.0f).SetUpdate(true).SetDelay(2f).OnComplete(() =>
                    {
                        _stateManager.SetState(State.PreGame);
                    });
                });
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