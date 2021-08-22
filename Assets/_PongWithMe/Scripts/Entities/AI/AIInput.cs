namespace PongWithMe
{
    public class AIInput : IInput
    {
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public bool IsPressingUp()
        {
            return Up;
        }

        public bool IsPressingDown()
        {
            return Down;
        }

        public bool IsPressingLeft()
        {
            return Left;
        }

        public bool IsPressingRight()
        {
            return Right;
        }
    }
}