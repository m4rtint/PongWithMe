using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe 
{
    public class BoardGenerator
    {
        private const float POSITION_LIMIT = 3f;
        private const float PADDING = 0.5f;
        
        private readonly List<Brick> _bricks = new List<Brick>();

        public List<Brick> Bricks => _bricks;

        public BoardGenerator()
        {
            for (float column = POSITION_LIMIT; column >= -POSITION_LIMIT; column -= PADDING)
            {
                for (float row = -POSITION_LIMIT; row <= POSITION_LIMIT; row += PADDING)
                {
                    var brick = new Brick();
                    brick.Position = new Vector3(row, column, 0);
                    _bricks.Add(brick);
                }
            }
        }
    }
}