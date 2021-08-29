using System.Collections.Generic;
using DG.Tweening;

namespace PongWithMe
{
    public class MutatorManager
    {
        private readonly List<BaseMutator> _listOfMutators = new List<BaseMutator>();

        public MutatorManager(
            List<IPaddle> players,
            IPaddleRotationMutator paddleRotator,
            GoalsManager goalsManager,
            IPlayerLives playerLives,
            Brick[] bricks)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players, paddleRotator));
            _listOfMutators.Add(new RebalanceLives(players, bricks, playerLives));
        }

        public void PickMutatorToActivate()
        {
            var mutator = _listOfMutators[1];
            StateManager.Instance.SetState(State.Animating);
            TimeScaleController.AnimatingTimeScale().OnComplete(() =>
            {
                mutator.ActivateMutator();
            });
        }
    }
}