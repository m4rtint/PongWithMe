using System;
using UnityEngine;

namespace PongWithMe
{
    public class AIPaddle : IPaddle
    {
        private enum AIPaddleState
        {
            Up,
            Down,
            Left,
            Right,
            Idle
        }
        
        private int _playerNumber;
        private IInput _aiInput;
        private Direction _direction;

        public IInput Input => _aiInput;
        public Direction PaddleDirection => _direction;
        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);

        public AIPaddle(IInput input, int playerNumber, Direction direction, GameObject
             ball)
        {
            if (ball == null)
            {
                PanicHelper.Panic(new Exception("AIPaddleBehaviour missing a ball"));
            }
            
            _playerNumber = playerNumber;
            _aiInput = input;
            _direction = direction;
        }

        public void OnFixedUpdate(Vector3 position)
        {
            
        }
    }
}