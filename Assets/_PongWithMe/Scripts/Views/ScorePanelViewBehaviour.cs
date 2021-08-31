using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class ScorePanelViewBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlayerScoreViewBehaviour[] _playerScorePanels = null;

        private Dictionary<IPaddle, PlayerScoreViewBehaviour> _dictionaryScores =
            new Dictionary<IPaddle, PlayerScoreViewBehaviour>();

        public void Initialize(List<IPaddle> players, int lives)
        {
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var playerScorePanel = _playerScorePanels[i];
                playerScorePanel.Initialize(player.PlayerColor, lives);
                _dictionaryScores.Add(player, playerScorePanel);
            }
        }

        public void PlayerWin(IPaddle paddle)
        {
            
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.ShowScore)
            {
                
            }
        }
    }
}