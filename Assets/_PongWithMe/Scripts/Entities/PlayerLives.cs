using System.Linq;

namespace PongWithMe
{
    public class PlayerLives
    {
        private readonly Brick[] _brickLives;

        public PlayerLives(Brick[] bricks)
        {
            _brickLives = bricks;
            // Requirements
            // Know how many lives each player has at all times
            // Can Destroy a life of choice

            // Keeps track of Brick => life
        }

        public int GetPlayerLives(int player)
        {
            return _brickLives.Count(brick => brick.PlayerOwned == player && brick.IsActive);
        }
    }
}