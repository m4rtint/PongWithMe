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

    public abstract class BaseMutator
    {
        public abstract void ActivateMutator();
    }

    public class RotatePaddles : BaseMutator
    {
        private GoalsManager _goalsManager = null;
        private List<IPaddle> _players = null;
        
        public RotatePaddles(
            GoalsManager goalsManager,
            List<IPaddle> players)
        {
            _goalsManager = goalsManager;
            _players = players;
        }

        private void RotatePlayerDirection()
        {
            
        }
        
        public override void ActivateMutator()
        {
            Debug.Log("Activate Rotator");
        }
    }
}