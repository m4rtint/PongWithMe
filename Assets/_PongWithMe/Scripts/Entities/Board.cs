namespace PongWithMe
{
    public class Board
    {
        private Brick[] _bricks;
        public Brick[] Bricks => _bricks;

        public Board(Brick[] bricks)
        {
            _bricks = bricks;
        }

        public void CleanUp()
        {
            _bricks = new Brick[]{};
        }
        
        public void Reset(Brick[] bricks)
        {
            _bricks = bricks;
        }
    }
}