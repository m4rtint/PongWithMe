
using PerigonGames;

namespace PongWithMe
{
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class PlayerDirectionMapper
    {
        private readonly Direction[] _playerDirections;

        public PlayerDirectionMapper(int amountOfPlayers)
        {
            _playerDirections = new Direction[amountOfPlayers];
        }

        public void SetPlayerDirection(int player, Direction direction)
        {
            _playerDirections[player] = direction;
        }

        public Direction? GetPlayerDirection(int player)
        {
            return _playerDirections.NullableGetElementAt(player);
        }
    }
}