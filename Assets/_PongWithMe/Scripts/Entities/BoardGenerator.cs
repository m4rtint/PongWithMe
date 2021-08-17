using System;
using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe 
{
    public class BoardGenerator
    {
        private const float POSITION_LIMIT = 3f;
        private const float PADDING = 0.5f;
        
        private readonly Brick[] _bricks;
        private int _maxNumberOfBricks = 0;

        public Brick[] Bricks => _bricks;


        public BoardGenerator(int amountOfPlayers)
        {
            SetMaxNumberOfBricks();
            var bricks = new List<Brick>();
            for (float column = POSITION_LIMIT; column >= -POSITION_LIMIT; column -= PADDING)
            {
                for (float row = -POSITION_LIMIT; row <= POSITION_LIMIT; row += PADDING)
                {
                    var brick = GenerateBrickToPlayer();
                    brick.Position = new Vector3(row, column, 0);
                    bricks.Add(brick);
                }
            }

            _bricks = bricks.ToArray();
        }

        private Brick GenerateBrickToPlayer()
        {
            var brick = new Brick();
            return brick;
        }

        private void SetMaxNumberOfBricks()
        {
            var oneSideOfBoard =((POSITION_LIMIT * 2) / PADDING) + 1;
            _maxNumberOfBricks = (int) Math.Ceiling(Math.Pow(oneSideOfBoard, 2));
        }
    }
}