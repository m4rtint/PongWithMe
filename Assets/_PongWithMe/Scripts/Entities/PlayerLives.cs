using System;
using System.Linq;
using PerigonGames;

namespace PongWithMe
{
    public interface IPlayerLives
    {
        void ForceBrickBreakOwnedBy(int player);
        event Action<int, int> OnBrickBreak;
    }
    
    public class PlayerLives : IPlayerLives
    {
        private readonly Brick[] _brickLives;
        
        public event Action<int, int> OnBrickBreak;

        public PlayerLives(Brick[] bricks)
        {
            _brickLives = bricks;
            foreach (var brick in bricks)
            {
                brick.OnBrickIsActiveSet += HandleOnBrickInactive;
            }
        }

        public int GetPlayerLives(int player)
        {
            return _brickLives.Count(brick => brick.PlayerOwned == player && brick.IsActive);
        }

        #region Delegate

        private void HandleOnBrickInactive(Brick brick, bool isActivate)
        {
            var playerLives = GetPlayerLives(brick.PlayerOwned);
            OnBrickBreak?.Invoke(brick.PlayerOwned, playerLives);
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