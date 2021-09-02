using System.Collections.Generic;
using System.Linq;
using PerigonGames;

namespace PongWithMe
{
    public class RebalanceLives : BaseMutator
    {
        private const int NUMBER_OF_PLAYERS = 4;
        
        private List<IPaddle> _players = null;
        private Board _board = null;
        private IPlayerLives _playerLives = null;
        private IRandomUtility _random = null;

        private Brick[] ActiveBricks => _board.Bricks.Where(brick => brick.IsActive).ToArray();
        
        public RebalanceLives(
            List<IPaddle> players, 
            Board board, 
            IPlayerLives playerLives,
            IRandomUtility randomUtility = null)
        {
            _players = players;
            _board = board;
            _playerLives = playerLives;
            _random = randomUtility ?? new RandomUtility();
        }


        public override void ActivateMutator()
        {
            Rebalance();
            StateManager.Instance.SetState(State.Play);
        }

        public override bool CanActivate()
        {
            return ActiveBricks.Length >= NUMBER_OF_PLAYERS;
        }

        private void Rebalance()
        {
            var equalLives = BalancedLives(ActiveBricks.Length);
            var playerOrder = GeneratePlayerBrickOrder(equalLives);
            PushExtraLivesOntoStack(playerOrder);
            RebuildBoard(playerOrder);
            ResurrectPlayers();
            _playerLives.ForceUpdatePlayerScores();
        }

        private void ResurrectPlayers()
        {
            foreach (var player in _players)
            {
                player.IsActive = true;
            }
        }

        private int[] BalancedLives(int numberOfBricks)
        {
            int[] lives = new int[NUMBER_OF_PLAYERS];
            int equalLives = numberOfBricks / NUMBER_OF_PLAYERS;
            for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
            {
                lives[i] = equalLives;
            }

            return lives;
        }

        private Stack<int> GeneratePlayerBrickOrder(int[] balancedLives)
        {
            var stackOfPlayerLives = new Stack<int>();

            while (balancedLives.Count(x => x == 0) != NUMBER_OF_PLAYERS)
            {
                var nextPlayer = _random.NextInt(0, NUMBER_OF_PLAYERS);
                if (balancedLives[nextPlayer] > 0)
                {
                    balancedLives[nextPlayer]--;
                    stackOfPlayerLives.Push(nextPlayer);
                }
            }

            return stackOfPlayerLives;
        }

        private void PushExtraLivesOntoStack(Stack<int> stackOfLives)
        {
            for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
            {
                stackOfLives.Push(i);
            }
        }

        private void RebuildBoard(Stack<int> playerOrder)
        {
            foreach (var brick in ActiveBricks)
            {
                var playerNumber = playerOrder.Pop();
                brick.PlayerOwned = playerNumber;
                brick.BrickColor = _players[playerNumber].PlayerColor;
            }
        }
    } 
}

