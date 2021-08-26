using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class MutatorManager
    {
        private List<BaseMutator> _listOfMutators = new List<BaseMutator>();

        public MutatorManager(
            List<IPaddle> players, 
            GoalsManager goalsManager)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players));
        }

        public void PickMutatorToActivate()
        {
            var mutator = _listOfMutators[0];
            mutator.ActivateMutator();
        }
    }
}