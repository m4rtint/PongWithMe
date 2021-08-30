using System.Collections;
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
            float timeToWait = 0;
            foreach (var brick in _board.Bricks)
            {
                var brickBehaviour = _pool.PopPooledObject(BrickPool.KEY);
                brickBehaviour.gameObject.SetActive(true);
                brickBehaviour.Initialize(brick);
                StartCoroutine(SetBrickColor(brick, timeToWait));
                timeToWait += 0.02f;
            }
        }

        private IEnumerator SetBrickColor(Brick brick, float delay)
        {
            yield return new WaitForSeconds(delay);
            brick.BrickColor = brick.BrickColor;
        }

        private void Awake()
        {
            _pool = GetComponent<BrickPool>();
            _pool.Initialize();
        }
    } 
}

