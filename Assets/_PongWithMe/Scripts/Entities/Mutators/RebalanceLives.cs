using System;
using System.Linq;

namespace PongWithMe
{
    public class RebalanceLives : BaseMutator
    {
        private IPaddle[] _players = null;
        private Brick[] _bricks = null;
        
        public RebalanceLives(IPaddle[] players, Brick[] bricks)
        {
            _players = players;
            _bricks = bricks;
        }
        
        public override void ActivateMutator()
        {
            
        }

        private void Rebalance()
        {
            var activeBricks = _bricks.Where(
                brick => brick.IsActive).ToArray();
            var equalLives = BalancedLives(_players.Length, activeBricks);
            
        }

        private int BalancedLives(double amountOfActivePlayers, Brick[] bricks)
        {
            return (int) Math.Floor(bricks.Length / amountOfActivePlayers);
        }
    } 
}

