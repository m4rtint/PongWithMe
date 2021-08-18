using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
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


        // The a template of the look of the bricks will be passed in in the future
        public BoardGenerator(int amountOfPlayers, bool[] boardTemplate = null)
        {
            SetMaxNumberOfBricks();
            boardTemplate ??= AllEnabledArray(_maxNumberOfBricks);
            
            var bricks = new List<Brick>();
            var index = 0;
            for (float column = POSITION_LIMIT; column >= -POSITION_LIMIT; column -= PADDING)
            {
                for (float row = -POSITION_LIMIT; row <= POSITION_LIMIT; row += PADDING)
                {
                    var brick = GenerateBrickToPlayer();
                    brick.Position = new Vector3(row, column, 0);
                    brick.IsActive = boardTemplate[index];
                    bricks.Add(brick);
                    index++;
                }
            }

            _bricks = bricks.ToArray();
        }

        private Brick GenerateBrickToPlayer()
        {
            var brick = new Brick();
            return brick;
        }

        private bool[] AllEnabledArray(int size)
        {
            var array = new bool[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = true;
            }

            return array;
        }

        private int[] SeparateLives(int amountOfPlayers)
        {
            int[] playerLives = new int[amountOfPlayers];
            for (int i = 0; i < amountOfPlayers; i++)
            {
                playerLives[i] = _maxNumberOfBricks / amountOfPlayers;
            }

            return playerLives;
        }

        private void SetMaxNumberOfBricks()
        {
            var oneSideOfBoard =((POSITION_LIMIT * 2) / PADDING) + 1;
            _maxNumberOfBricks = (int) Math.Ceiling(Math.Pow(oneSideOfBoard, 2));
        }
    }
}