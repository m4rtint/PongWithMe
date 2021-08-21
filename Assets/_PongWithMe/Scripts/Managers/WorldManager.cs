using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        
        private Board _board = null;
        private PlayerLives _playerLives = null;
        
        private void Start()
        {
            new ColorPalette();
            var boardGenerator = new BoardGenerator(2);
            _playerLives = new PlayerLives(boardGenerator.Bricks);
            _board = new Board(boardGenerator.Bricks);
            _bricksBehaviour.Initialize(_board);
            _ballBehaviour.Initialize();
        }
    } 
}

