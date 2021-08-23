using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private PlayersManager _playersManager = null;
        [SerializeField] private GoalsManager _goalsManager = null;
        
        private Board _board = null;
        private PlayerLives _playerLives = null;
        
        private void Start()
        {
            new ColorPalette();
            _playersManager.Initialize(_ballBehaviour);
            SetupGoals();
            
            var boardGenerator = new BoardGenerator(AMOUNT_OF_PLAYERS);
            _playerLives = new PlayerLives(boardGenerator.Bricks);
            _board = new Board(boardGenerator.Bricks);
            _bricksBehaviour.Initialize(_board);
            _ballBehaviour.Initialize();
        }

        private void SetupGoals()
        {
            foreach (var player in _playersManager.Players)
            {
                _goalsManager.Set(player);
            }
        }
    } 
}

