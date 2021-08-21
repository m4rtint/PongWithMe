using System;
using UnityEngine;

namespace PongWithMe
{
    public class Brick
    {
        private Vector3 _position = Vector3.zero;
        private Color _brickColor;
        private bool _isActive = false;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPositionSet?.Invoke(value);
            }
        }

        public Color BrickColor
        {
            get => _brickColor;
            set
            {
                _brickColor = value;
                OnColorSet?.Invoke(value);
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnIsActiveSet?.Invoke(value);
            }
        }

        public event Action<Color> OnColorSet;

        public event Action<Vector3> OnPositionSet;

        public event Action<bool> OnIsActiveSet;
    }
}

