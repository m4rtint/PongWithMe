using UnityEngine;

namespace PongWithMe
{
    public interface IPaddle
    {
        int PlayerNumber { get; }
        IInput Input { get; }
        Direction PaddleDirection { get; }
        Color PlayerColor { get; }
    }
    
    public class PlayerPaddle : IPaddle
    {
        private readonly int _playerNumber;
        private IInput _playerInput;
        private Direction _direction;
        
        public IInput Input => _playerInput;
        public Direction PaddleDirection => _direction;
        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);
        public int PlayerNumber => _playerNumber;

        
        public PlayerPaddle(IInput input, int playerNumber, Direction direction)
        {
            _playerNumber = playerNumber;
            _playerInput = input;
            _direction = direction;
        }
    }

}

