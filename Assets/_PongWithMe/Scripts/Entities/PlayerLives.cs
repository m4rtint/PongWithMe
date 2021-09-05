using System;
using System.Linq;
using PerigonGames;

namespace PongWithMe
{
    public interface IPlayerLives
    {
        void ForceBrickBreakOwnedBy(int player);
        event Action<int, int> OnBrickBreak;
        void ForceUpdatePlayerScores();
        event Action OnForcePlayerScoresUpdate; 
        int GetPlayerLives(int player);
    }
    
    public class PlayerLives : IPlayerLives
    {
        private readonly int _amountOfPlayers;

        public event Action<int, int> OnBrickBreak;
        public event Action OnForcePlayerScoresUpdate;

        private Board _board;

        public PlayerLives(Board board, int amountOfPlayers)
        {
            _amountOfPlayers = amountOfPlayers;
            _board = board;
        }

        public int GetPlayerLives(int player)
        {
            return _board.Bricks.Count(brick => brick.PlayerOwned == player && brick.IsActive);
        }
        
        public void ForceUpdatePlayerScores()
        {
            OnForcePlayerScoresUpdate?.Invoke();
        }

        public void CleanUp()
        {
            foreach (var brick in _board.Bricks)
            {
                brick.OnBrickIsActiveSet -= HandleOnBrickInactive;
            }
        }

        public void Reset()
        {
            foreach (var brick in _board.Bricks)
            {
                brick.OnBrickIsActiveSet += HandleOnBrickInactive;
            }
        }

        private bool HasGameEnded()
        {
            var playersAlive = 0;
            for (int i = 0; i < _amountOfPlayers; i++)
            {
                if (GetPlayerLives(i) > 0)
                {
                    playersAlive++;
                }
            }

            return playersAlive == 1;
        }

        #region Delegate

        private void HandleOnBrickInactive(Brick brick, bool isActivate)
        {
            var playerLives = GetPlayerLives(brick.PlayerOwned);
            OnBrickBreak?.Invoke(brick.PlayerOwned, playerLives);
            if (HasGameEnded() && StateManager.Instance.GetState() == State.Play)
            {
                StateManager.Instance.SetState(State.EndRound);
            }
        }
        #endregion
        
        #region Interface
        
        public void ForceBrickBreakOwnedBy(int player)
        {
            var arrayOfPlayerLives = _board.Bricks.Where(brick => brick.PlayerOwned == player && brick.IsActive).ToArray();
            var random = new RandomUtility();
            if (random.NextTryGetElement(arrayOfPlayerLives, out var brick))
            {
                brick.IsActive = false;
            }
        }
        #endregion
    }
}