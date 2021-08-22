using System;
using UnityEngine;

namespace PongWithMe
{
    public class AIPaddle : IPaddle
    {
        private readonly int _playerNumber;
        private readonly GameObject _ball;
        private readonly AIInput _aiInput;
        private readonly IInput _input;
        private readonly Direction _paddleDirection;

        private bool IsPlacementHorizontal => _paddleDirection == Direction.Left || _paddleDirection == Direction.Right;

        public IInput Input => _input;
        public Direction PaddleDirection => _paddleDirection;

        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);

        public AIPaddle(IInput input, int playerNumber, Direction direction, GameObject
             ball)
        {
            if (ball == null)
            {
                PanicHelper.Panic(new Exception("AIPaddleBehaviour missing a ball"));
            }

            _ball = ball;
            _playerNumber = playerNumber;
            _input = input;
            _aiInput = input as AIInput;
            _paddleDirection = direction;
        }

        public void OnFixedUpdate(Vector3 position)
        {
            _aiInput.ResetInput();
            if (IsPlacementHorizontal)
            {
                HandleVertical(position);
            }
            else
            {
                HandleHorizontal(position);
            }
        }

        private void HandleHorizontal(Vector3 paddlePosition)
        {
        }

        private void HandleVertical(Vector3 paddlePosition)
        {
            if (paddlePosition.y < _ball.transform.position.y)
            {
                _aiInput.Up = true;
            } 
            else if (paddlePosition.y > _ball.transform.position.y)
            {
                _aiInput.Down = true;
            }
        }
    }
}