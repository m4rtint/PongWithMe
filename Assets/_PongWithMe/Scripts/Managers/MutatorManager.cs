using System.Collections.Generic;

namespace PongWithMe
{
    public class MutatorManager
    {
        private List<BaseMutator> _listOfMutators = new List<BaseMutator>();

        public MutatorManager(
            List<IPaddle> players,
            IPaddleRotationMutator paddleRotator,
            GoalsManager goalsManager)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players, paddleRotator));
        }

        public void PickMutatorToActivate()
        {
            var mutator = _listOfMutators[0];
            mutator.ActivateMutator();
        }
    }
}