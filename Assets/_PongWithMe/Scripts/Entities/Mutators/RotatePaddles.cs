using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class RotatePaddles : BaseMutator
    {
        private const int NUMBER_OF_DIRECTIONS = 4;
        
        private GoalsManager _goalsManager = null;
        private List<IPaddle> _players = null;

        public RotatePaddles(
            GoalsManager goalsManager,
            List<IPaddle> players)
        {
            _goalsManager = goalsManager;
            _players = players;
        }

        private void RotatePlayerClockWiseDirection()
        {
            foreach (var player in _players)
            {
                player.PaddleDirection = RotateDirectionClockWise(player.PaddleDirection);
            }
        }

        public override void ActivateMutator()
        {
            Debug.Log("Activate Rotator");
        }

        private Direction RotateDirectionClockWise(Direction direction)
        {
            int integerDirection = (int) direction + 1;
            if (integerDirection > NUMBER_OF_DIRECTIONS - 1)
            {
                integerDirection = 0;
            }

            return (Direction) integerDirection;
        }
        
        private Direction RotateDirectionAntiClockWise(Direction direction)
        {
            int integerDirection = (int) direction - 1;
            if (integerDirection < 0)
            {
                integerDirection = NUMBER_OF_DIRECTIONS - 1;
            }

            return (Direction) integerDirection;
        }
    }
}