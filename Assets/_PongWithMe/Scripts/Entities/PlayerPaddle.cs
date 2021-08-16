using Rewired;
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
        public Color PlayerColor
        {
            get
            {
                switch (PlayerNumber)
                {
                    case 0:
                        return ColorPalette.PastelBlue;
                    case 1:
                        return ColorPalette.PastelRed;
                    case 2:
                        return ColorPalette.PastelGreen;
                    case 3:
                        return ColorPalette.PastelYellow;
                }

                return ColorPalette.PastelGrey;
            }
        }


        public PlayerPaddle(IInput input, int number = 0, Direction direction = Direction.Left)
        {
            PlayerNumber = number;
            _playerInput = input;
            _direction = direction;
        }

    }
}

