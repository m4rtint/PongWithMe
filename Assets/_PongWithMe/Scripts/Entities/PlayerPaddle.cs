using UnityEngine;

namespace PongWithMe
{
    public interface IPaddle
    {
        IInput Input { get; }
        Direction PaddleDirection { get; }
        Color PlayerColor { get; }
    }
    
    public class PlayerPaddle : IPaddle
    {
        public int PlayerNumber;
        
        private IInput _playerInput;
        private Direction _direction;
        
        public IInput Input => _playerInput;
        public Direction PaddleDirection => _direction;
        public Color PlayerColor => ColorPalette.PlayerColor(PlayerNumber);
        
        public PlayerPaddle(IInput input, int playerNumber, Direction direction = Direction.Left)
        {
            PlayerNumber = playerNumber;
            _playerInput = input;
            _direction = direction;
        }
    }

}

