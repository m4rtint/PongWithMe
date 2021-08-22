using System;
using System.Linq;

namespace PongWithMe
{
    public class PlayerLives
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
    }
}