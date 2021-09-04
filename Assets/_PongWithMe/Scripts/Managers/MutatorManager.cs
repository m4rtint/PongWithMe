using System;
using System.Collections.Generic;
using DG.Tweening;
using PerigonGames;

namespace PongWithMe
{
    public class MutatorManager
    {
        private readonly List<BaseMutator> _listOfMutators = new List<BaseMutator>();

        public event Action<string> OnMutatorPicked;

        public MutatorManager(
            List<IPaddle> players,
            IPaddleRotationMutator paddleRotator,
            GoalsManager goalsManager,
            IPlayerLives playerLives,
            IBall ball,
            Board board,
            Splatter splatter,
            Portals portals)
        {
            _listOfMutators.Add(new RotatePaddles(goalsManager, players, paddleRotator));
            _listOfMutators.Add(new RebalanceLives(players, board, playerLives));
            _listOfMutators.Add(new ActivateForceField(ball, goalsManager, players));
            _listOfMutators.Add(new SplatterBoard(splatter));
            _listOfMutators.Add(new PortalsMutator(portals));
        }

        public void PickMutatorToActivate()
        {
            var mutator = PickRandomMutator();
            OnMutatorPicked?.Invoke(mutator.Announcement);
            
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
            BaseMutator mutator = null;
            do
            {
                var index = random.NextInt(0, _listOfMutators.Count);
                mutator = _listOfMutators.NullableGetElementAt(index);
            } 
            while (!mutator.CanActivate());

            return mutator;
        }
    }
}