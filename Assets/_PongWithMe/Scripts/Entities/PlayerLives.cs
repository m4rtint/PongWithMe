using System;
using System.Linq;
using PerigonGames;

namespace PongWithMe
{
    public interface IPlayerLives
    {
        void ForceBrickBreakOwnedBy(int player);
        event Action<int, int> OnBrickBreak;
        int GetPlayerLives(int player);
    }
    
    public class PlayerLives : IPlayerLives
    {
        private readonly Brick[] _brickLives;
        private readonly int _amountOfPlayers;

        public event Action<int, int> OnBrickBreak;

        public PlayerLives(Brick[] bricks, int amountOfPlayers)
        {
            _brickLives = bricks;
            _amountOfPlayers = amountOfPlayers;
            foreach (var brick in bricks)
            {
                brick.OnBrickIsActiveSet += HandleOnBrickInactive;
            }
        }

        public int GetPlayerLives(int player)
        {
            return _brickLives.Count(brick => brick.PlayerOwned == player && brick.IsActive);
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
            if (HasGameEnded())
            {
                StateManager.Instance.SetState(State.EndGame);
            }
        }
        #endregion
        
        #region Interface
        
        public void ForceBrickBreakOwnedBy(int player)
        {
            var arrayOfPlayerLives = _brickLives.Where(brick => brick.PlayerOwned == player && brick.IsActive).ToArray();
            var random = new RandomUtility();
            if (random.NextTryGetElement(arrayOfPlayerLives, out var brick))
            {
                brick.IsActive = false;
            }
        }
        #endregion
    }
}