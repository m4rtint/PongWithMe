using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class BoardTemplates
    {
        public static bool[] OddBricks(int size)
        {
            var array = new bool[size];
            for (int i = 0; i < size; i++)
            {
                if (i % 2 == 1)
                {
                    array[i] = true;
                }
            }
            
            return array;
        }
    }
}
