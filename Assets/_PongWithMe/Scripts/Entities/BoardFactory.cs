using System;
using System.Collections.Generic;
using System.Linq;
using PerigonGames;
using UnityEngine;

namespace PongWithMe 
{
    public static class BoardFactory
    {
        private const float POSITION_LIMIT = 3f;
        private const float PADDING = 0.5f;

        // The a template of the look of the bricks will be passed in in the future
        public static Brick[] Build(int amountOfPlayers, bool[] boardTemplate = null)
        {
            var maxNumberOfBricks = SetMaxNumberOfBricks();
            boardTemplate ??= BoardTemplates.OddBricks(maxNumberOfBricks);
            ValidateBoard(amountOfPlayers, boardTemplate);
            var separatedLives = BalancedLives(amountOfPlayers, boardTemplate);
            var playerOrder = GeneratePlayerBrickOrder(separatedLives, amountOfPlayers);
            return BuildBoard(playerOrder, boardTemplate);
        }

        private static Brick[] BuildBoard(Stack<int> playerOrder, bool[] boardTemplate)
        {
            var bricks = new List<Brick>();
            var index = 0;
            for (var column = POSITION_LIMIT; column >= -POSITION_LIMIT; column -= PADDING)
            {
                for (var row = -POSITION_LIMIT; row <= POSITION_LIMIT; row += PADDING)
                {
                    if (boardTemplate.NullableGetElementAt(index))
                    {
                        var brick = GenerateBrickToPlayer(new Vector3(row, column, 0), playerOrder.Pop());
                        brick.IsActive = true;
                        bricks.Add(brick);
                    }
                    
                    index++;
                }
            }

            return bricks.ToArray();
        }

        private static Brick GenerateBrickToPlayer(Vector3 position, int player)
        {
            var brick = new Brick {Position = position, BrickColor = ColorPalette.PlayerColor(player), PlayerOwned = player};
            return brick;
        }

        private static Stack<int> GeneratePlayerBrickOrder(int[] separatedLives, int numberOfPlayers)
        {
            var randomGenerator = new RandomUtility();
            var stackOfPlayerLives = new Stack<int>();
            while (separatedLives.Count(x => x == 0) != numberOfPlayers)
            {
                var nextPlayer = randomGenerator.NextInt(0, numberOfPlayers);
                if (separatedLives[nextPlayer] > 0)
                {
                    separatedLives[nextPlayer]--;
                    stackOfPlayerLives.Push(nextPlayer);
                }
            }

            return stackOfPlayerLives;
        }

        private static int[] BalancedLives(int amountOfPlayers, bool[] boardTemplate)
        {
            int numberOfActiveBricks = boardTemplate.Count(x => x);
            int[] playerLives = new int[amountOfPlayers];
            for (int i = 0; i < amountOfPlayers; i++)
            {
                playerLives[i] = numberOfActiveBricks / amountOfPlayers;
            }

            return playerLives;
        }

        private static int SetMaxNumberOfBricks()
        {
            var oneSideOfBoard =((POSITION_LIMIT * 2) / PADDING) + 1;
            return (int) Math.Ceiling(Math.Pow(oneSideOfBoard, 2));
        }

        private static void ValidateBoard(int numberOfPlayers, bool[] boardTemplate)
        {
            var numberOfActiveBricks = boardTemplate.Count(b => b);
            var remainder = numberOfActiveBricks % numberOfPlayers;
            if (remainder != 0)
            {
                PanicHelper.Panic(new Exception($"Template of {numberOfActiveBricks} does not divide by {numberOfPlayers}, off by {remainder}"));
            }
        }
    }
}