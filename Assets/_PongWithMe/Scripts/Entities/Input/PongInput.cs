using System;
using Rewired;
using UnityEngine;

namespace PongWithMe
{
    public class PongInput : IInput
    {
        private const float INPUT_TOLERANCE = 0.001F;
        private const string VERTICAL_AXIS = "Vertical";
        private const string HORIZONTAL_AXIS = "Horizontal";
        
        private Player _playerInput;

        public PongInput(int playerNumber)
        {
            _playerInput = ReInput.players.GetPlayer(playerNumber);
        }
        
        public bool IsPressingUp()
        {
            return Math.Abs(_playerInput.GetAxis(VERTICAL_AXIS) - 1) < INPUT_TOLERANCE;
        }

        public bool IsPressingDown()
        {
            return Math.Abs(_playerInput.GetAxis(VERTICAL_AXIS) - (-1)) < INPUT_TOLERANCE;
        }

        public bool IsPressingLeft()
        {
            return Math.Abs(_playerInput.GetAxis(HORIZONTAL_AXIS) - (-1)) < INPUT_TOLERANCE;
        }

        public bool IsPressingRight()
        {
            return Math.Abs(_playerInput.GetAxis(HORIZONTAL_AXIS) - 1) < INPUT_TOLERANCE;
        }
    }
}
