using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public static class AIDifficulty
    {
        public enum Difficulty
        {
            SIMPLE,
            EASY,
            MEDIUM,
            HARD
        }

        public static float DifficultyTolerance(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.SIMPLE:
                    return 2.0f;
                case Difficulty.EASY:
                    return 1.6f;
                case Difficulty.MEDIUM:
                    return 1.25f;
                case Difficulty.HARD:
                    return 0.75f;
                default:
                    return 1.75f;
            }
        }
    }
}