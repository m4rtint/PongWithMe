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
        private readonly float _tolerance;

        private bool IsMovementVertical => _paddleDirection == Direction.Left || _paddleDirection == Direction.Right;

        public IInput Input => _input;
        public Direction PaddleDirection => _paddleDirection;

        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);

        public AIPaddle(IInput input, 
            int playerNumber, 
            Direction direction, 
            GameObject ball,
            AIDifficulty.Difficulty difficulty = AIDifficulty.Difficulty.HARD)
        {
            if (ball == null)
            {
                PanicHelper.Panic(new Exception("AIPaddleBehaviour missing a ball"));
            }

            _tolerance = AIDifficulty.DifficultyTolerance(difficulty);
            _ball = ball;
            _playerNumber = playerNumber;
            _input = input;
            _aiInput = input as AIInput;
            _paddleDirection = direction;
        }

        public void OnFixedUpdate(Vector3 position)
        {
            if (IsMovementVertical)
            {
                HandleVertical(position);
            }
            else
            {
                HandleHorizontalMovement(position);
            }
        }

        private void HandleHorizontalMovement(Vector3 paddlePosition)
        {
            var displacement = paddlePosition.x - _ball.transform.position.x;
            var goLeft = displacement < 0;
            var shouldMove = Mathf.Abs(displacement) > _tolerance;
            if (goLeft && shouldMove)
            {
                _aiInput.Left = false;
                _aiInput.Right = true;
            } 
            else if (!goLeft && shouldMove)
            {
                _aiInput.Right = false;
                _aiInput.Left = true;
            }
            else
            {
                _aiInput.ResetInput();
            }
        }

        private void HandleVertical(Vector3 paddlePosition)
        {
            var displacement = paddlePosition.y - _ball.transform.position.y;
            var goUp = displacement < 0;
            var shouldMove = Mathf.Abs(displacement) > _tolerance;
            if (goUp && shouldMove)
            {
                _aiInput.Down = false;
                _aiInput.Up = true;
            } 
            else if (!goUp && shouldMove)
            {
                _aiInput.Up = false;
                _aiInput.Down = true;
            }
            else
            {
                _aiInput.ResetInput();
            }
        }
    }
}