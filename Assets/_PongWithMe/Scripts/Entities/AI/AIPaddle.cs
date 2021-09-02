using System;
using UnityEngine;

namespace PongWithMe
{
    public class AIPaddle : IPaddle
    {
        private readonly int _playerNumber;
        private readonly IBall _ball;
        private readonly AIInput _aiInput;
        private readonly IInput _input;
        private readonly float _tolerance;
        private readonly float _moveLimit;
        private readonly IStateManager _stateManager;
        
        private bool _isActive = true;
        private Direction _paddleDirection;

        private bool IsMovementVertical => _paddleDirection == Direction.Left || _paddleDirection == Direction.Right;


        public event Action<bool> OnIsActiveUpdated;
        public event Action<Direction> OnDirectionChanged;

        public IInput Input => _input;
        public Direction PaddleDirection
        {
            get => _paddleDirection;
            set
            {
                _paddleDirection = value;
                OnDirectionChanged?.Invoke(value);
            }
        }

        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);

        public int PlayerNumber => _playerNumber;
        
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnIsActiveUpdated?.Invoke(value);
            }
        }

        public AIPaddle(IInput input, 
            int playerNumber, 
            Direction direction, 
            IBall ball,
            AIDifficulty.Difficulty difficulty = AIDifficulty.Difficulty.HARD, 
            float moveLimit = PaddleMovementBehaviour.MOVE_LIMIT,
            IStateManager stateManager = null)
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
            _moveLimit = moveLimit;
            _stateManager = stateManager ?? StateManager.Instance;
        }
        
        public void Reset()
        {
            IsActive = true;
        }

        public void CleanUp()
        {
            IsActive = false;
        }

        public void OnUpdate(Vector3 position)
        {
            if (_stateManager.GetState() != State.Play)
            {
                return;
            }

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
            var displacement = paddlePosition.x - _ball.GetPosition.x;
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
                MoveAwayIfUnderPaddle(paddlePosition);
            }
        }

        private void HandleVertical(Vector3 paddlePosition)
        {
            var displacement = paddlePosition.y - _ball.GetPosition.y;
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
                MoveAwayIfUnderPaddle(paddlePosition);
            }
        }

        private void MoveAwayIfUnderPaddle(Vector3 paddlePosition)
        {
            switch (_paddleDirection)
            {
                case Direction.Top:
                case Direction.Bottom:
                    if (IsUnderPaddle(paddlePosition))
                    {
                        MoveAwayFromBall(paddlePosition);
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    if (IsUnderPaddle(paddlePosition))
                    {
                        MoveAwayFromBall(paddlePosition);
                    }
                    break;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    return;
            }
        }

        private bool IsUnderPaddle(Vector3 paddlePosition)
        {
            switch (_paddleDirection)
            {
                case Direction.Top:
                    return _ball.GetPosition.y > paddlePosition.y;
                case Direction.Bottom:
                    return _ball.GetPosition.y < paddlePosition.y;
                case Direction.Left:
                    return _ball.GetPosition.x < paddlePosition.x;
                case Direction.Right:
                    return _ball.GetPosition.x > paddlePosition.x;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    return false;
            }
        }

        private void MoveAwayFromBall(Vector3 paddlePosition)
        {
            switch (_paddleDirection)
            {
                case Direction.Top:
                case Direction.Bottom:
                    if (paddlePosition.x < _moveLimit / 2)
                    {
                        _aiInput.Right = true;
                    }
                    else
                    {
                        _aiInput.Left = true;
                    }

                    break;
                case Direction.Left:
                case Direction.Right:
                    if (paddlePosition.y < _moveLimit / 2)
                    {
                        _aiInput.Up = true;
                    }
                    else
                    {
                        _aiInput.Down = true;
                    }
                    break;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    break;
            }
        }
    }
}