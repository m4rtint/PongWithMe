using System;
using UnityEngine;

namespace PongWithMe
{
    public class Brick
    {
        private Vector3 _position = Vector3.zero;
        private Color _brickColor;
        private bool _isActive = false;
        
        public event Action<Brick, Color> OnBrickColorSet;
        public event Action<Brick, Vector3> OnBrickPositionSet;
        public event Action<Brick, bool> OnBrickIsActiveSet;
        
        public int PlayerOwned { get; set; }
        

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnBrickPositionSet?.Invoke(this, value);
            }
        }

        public Color BrickColor
        {
            get => _brickColor;
            set
            {
                _brickColor = value;
                OnBrickColorSet?.Invoke(this, value);
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnBrickIsActiveSet?.Invoke(this, value);
            }
        }

    }
}

