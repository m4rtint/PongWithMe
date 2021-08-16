using PerigonGames;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(BrickPool))]
    public class BricksBehaviour : MonoBehaviour
    {
        private BrickPool _pool = null;
        private Board _board = null;

        public void Initialize(Board board)
        {
            _board = board;
            SetupBricks();
        }

        private void SetupBricks()
        {
            foreach (var brick in _board.Bricks)
            {
                var brickBehaviour = _pool.PopPooledObject(BrickPool.KEY);
                brickBehaviour.gameObject.SetActive(true);
                brickBehaviour.Initialize(brick);
                brick.BrickColor = RandomColor(brick.GetHashCode());
            }
        }

        private Color RandomColor(int hash)
        {
            var random = new RandomUtility(hash);
            var number = random.NextInt(0, 4);
            switch (number)
            {
                case 0:
                    return ColorPalette.PastelBlue;
                case 1:
                    return ColorPalette.PastelGreen;
                case 2:
                    return ColorPalette.PastelRed;
                case 3:
                    return ColorPalette.PastelYellow;
            }

            return ColorPalette.PastelBlue;
        }
 
        private void Awake()
        {
            _pool = GetComponent<BrickPool>();
            _pool.Initialize();
        }
    } 
}

