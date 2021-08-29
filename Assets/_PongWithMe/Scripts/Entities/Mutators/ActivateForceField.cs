using System.Collections.Generic;
using System.Linq;
using PerigonGames;

namespace PongWithMe
{
    public class ActivateForceField : BaseMutator
    {
        private const int FORCE_FIELD_LIVES = 2;
        private IBall _ball = null;
        private GoalsManager _goalsManager = null;
        private List<IPaddle> _players = null;
        
        public ActivateForceField(IBall ball, GoalsManager goalsManager, List<IPaddle> players)
        {
            _ball = ball;
            _goalsManager = goalsManager;
            _players = players;
        }

        public override void ActivateMutator()
        {
            var lastHit = _ball.LastHitFrom;
            var random = new RandomUtility();
            while (IsLastHitNotActive(lastHit))
            {
                lastHit = (Direction) random.NextInt(0, 4);
            }
            
            _goalsManager.ActivateForceField(lastHit, FORCE_FIELD_LIVES);
        }

        private bool IsLastHitNotActive(Direction lastHit)
        {
            return _players.Any(player => player.PaddleDirection == lastHit && !player.IsActive);
        }
    }
}