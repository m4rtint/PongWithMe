using System;
using UnityEngine;

namespace PongWithMe
{
    public interface IPaddle
    {
        int PlayerNumber { get; }
        IInput Input { get; }
        Direction PaddleDirection { get; set; }
        Color PlayerColor { get; }
        bool IsActive { get; set; }

        void Reset();

        event Action<bool> OnIsActiveUpdated;
        event Action<Direction> OnDirectionChanged;
    }
    
    public class PlayerPaddle : IPaddle
    {
        private readonly int _playerNumber;
        private IInput _playerInput;
        private readonly Direction _originalDirection;
        
        private Direction _direction;
        private bool _isActive = true;

        public event Action<bool> OnIsActiveUpdated;
        public event Action<Direction> OnDirectionChanged;

        public IInput Input => _playerInput;

        public Color PlayerColor => ColorPalette.PlayerColor(_playerNumber);
        public int PlayerNumber => _playerNumber;

        public Direction PaddleDirection
        {
            get => _direction;
            set
            {
                _direction = value;
                OnDirectionChanged?.Invoke(value);
            }
        }
        
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnIsActiveUpdated?.Invoke(value);
            }
        }

        public void Reset()
        {
            IsActive = true;
            PaddleDirection = _originalDirection;
        }

        public PlayerPaddle(IInput input, int playerNumber, Direction direction)
        {
            _playerNumber = playerNumber;
            _playerInput = input;
            _direction = direction;
            _originalDirection = direction;
        }
        
        public PlayerPaddle(int playerNumber, Direction direction)
        {
            _playerNumber = playerNumber;
            _direction = direction;
            _originalDirection = direction;
        }
    }

}

