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
            IBall ball,
            Brick[] bricks)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players, paddleRotator));
            _listOfMutators.Add(new RebalanceLives(players, bricks, playerLives));
            _listOfMutators.Add(new ActivateForceField(ball, goalsManager, players));
        }

        public void PickMutatorToActivate()
        {
            var mutator = _listOfMutators[2];
            
            //TODO - the tween needs to be handled else where
            StateManager.Instance.SetState(State.Animating);
            TimeScaleController.AnimatingTimeScale().OnComplete(() =>
            {
                mutator.ActivateMutator();
            });
        }
    }
}