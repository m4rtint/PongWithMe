using System.Collections.Generic;
using DG.Tweening;
using PerigonGames;

namespace PongWithMe
{
    public class RotatePaddles : BaseMutator
    {
        private const int NUMBER_OF_DIRECTIONS = 4;
        
        private GoalsManager _goalsManager = null;
        private List<IPaddle> _players = null;
        private IRandomUtility _randomUtility = null;
        private IPaddleRotationMutator _paddleRotator = null;
        
        public RotatePaddles(
            GoalsManager goalsManager,
            List<IPaddle> players,
            IPaddleRotationMutator rotationMutator,
            IRandomUtility random = null)
        {
            _goalsManager = goalsManager;
            _players = players;
            _paddleRotator = rotationMutator;
            _randomUtility = random ?? new RandomUtility();
        }
        
        public override void ActivateMutator()
        {
            var coinFlip = _randomUtility.CoinFlip();
            RotatePlayerDirection(coinFlip);
            _goalsManager.Set(_players);
            _paddleRotator.RotatePaddles(coinFlip).OnComplete(() =>
            {
                StateManager.Instance.SetState(State.Play);
            });
        }
        
        private void RotatePlayerDirection(bool isRotateClockwise)
        {
            foreach (var player in _players)
            {
                player.PaddleDirection = isRotateClockwise
                    ? RotateDirectionClockWise(player.PaddleDirection)
                    : RotateDirectionAntiClockWise(player.PaddleDirection);
            }
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