using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        
        private Board _board = null;
        
        private void Start()
        {
            new ColorPalette();
            var boardGenerator = new BoardGenerator();
            _board = new Board(boardGenerator.Bricks.ToArray());
            _bricksBehaviour.Initialize(_board);
            _ballBehaviour.Initialize();
        }
    } 
}

