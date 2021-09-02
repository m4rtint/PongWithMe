using System.Collections;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(BrickPool))]
    public class BricksBehaviour : MonoBehaviour
    {
        private const float RATE_OF_SPAWN = 0.02F;
        private BrickPool _pool = null;
        private Board _board = null;

        public void Initialize(Board board)
        {
            _board = board;
            SetupBricks();
        }
        
        public void CleanUp()
        {
            _pool.CleanUp();
        }

        public void Reset()
        { 
            SetupBricks();
        }
        

        private void SetupBricks()
        {
            float timeToWait = 0;
            foreach (var brick in _board.Bricks)
            {
                _pool.SpawnBrickWith(brick);
                StartCoroutine(SetBrickColor(brick, timeToWait));
                timeToWait += RATE_OF_SPAWN;
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

