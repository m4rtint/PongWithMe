using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private PlayersManager _playersManager = null;
        
        private Board _board = null;
        private PlayerLives _playerLives = null;
        
        private void Start()
        {
            new ColorPalette();
            var boardGenerator = new BoardGenerator(AMOUNT_OF_PLAYERS);
            _playerLives = new PlayerLives(boardGenerator.Bricks);
            _board = new Board(boardGenerator.Bricks);
            _bricksBehaviour.Initialize(_board);
            _ballBehaviour.Initialize();
            _playersManager.Initialize(_ballBehaviour);
        }
    } 
}

