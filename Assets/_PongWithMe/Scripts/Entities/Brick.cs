using System;
using UnityEngine;

namespace PongWithMe
{
    public class Brick
    {
        private Vector3 _position = Vector3.zero;
        private Color _brickColor;
        
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

        public event Action<Color> OnColorSet;

        public event Action<Vector3> OnPositionSet;
    }
}

