using System.Collections.Generic;
using DG.Tweening;
using PerigonGames;

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
            Brick[] bricks,
            Splatter splatter)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players, paddleRotator));
            _listOfMutators.Add(new RebalanceLives(players, bricks, playerLives));
            _listOfMutators.Add(new ActivateForceField(ball, goalsManager, players));
            _listOfMutators.Add(new SplatterBoard(splatter));
        }

        public void PickMutatorToActivate()
        {
            var mutator = PickRandomMutator();
            
            //TODO - the tween needs to be handled else where
            StateManager.Instance.SetState(State.Animating);
            TimeScaleController.AnimatingTimeScale().OnComplete(() =>
            {
                mutator.ActivateMutator();
            });
        }

        private BaseMutator PickRandomMutator()
        {
            var random = new RandomUtility();
            var index = random.NextInt(0, _listOfMutators.Count);
            BaseMutator mutator = null;
            do
            {
                mutator = _listOfMutators.NullableGetElementAt(index);
            } 
            while (!mutator.CanActivate());

            return mutator;
        }
    }
}