using UnityEngine;

namespace PongWithMe
{
    public class PlayerPaddle
    {
        public int PlayerNumber;
        
        private IInput _playerInput;
        private Direction _direction;
        
        public IInput PlayerInput => _playerInput;
        public Direction PaddleDirection => _direction;
        public Color PlayerColor => ColorPalette.PlayerColor(PlayerNumber);



        public PlayerPaddle(IInput input, int number = 0, Direction direction = Direction.Left)
        {
            PlayerNumber = number;
            _playerInput = input;
            _direction = direction;
        }
    }

    public struct PlayerProperties
    {
        public int PlayerNumber;
        public Color PlayerColor;
    }
}
