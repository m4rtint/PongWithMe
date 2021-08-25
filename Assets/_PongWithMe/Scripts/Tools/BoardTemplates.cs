namespace PongWithMe
{
    public static class BoardTemplates
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
        
        
        public static bool[] FourLives(int size)
        {
            var array = new bool[size];
            array[0] = true;
            array[1] = true;
            array[2] = true;
            array[3] = true;
            return array;
        }
    }
}
