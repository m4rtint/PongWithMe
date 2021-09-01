using System.Collections;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(BrickPool))]
    public class BricksBehaviour : MonoBehaviour
    {
        private const float RATE_OF_SPAWN = 0.02F;
        private BrickPool _pool = null;

        public void Initialize(Brick[] bricks)
        {
            SetupBricks(bricks);
        }
        
        public void CleanUp()
        {
            _pool.CleanUp();
        }

        public void Reset(Brick[] bricks)
        { 
            SetupBricks(bricks);
        }
        

        private void SetupBricks(Brick[] bricks)
        {
            float timeToWait = 0;
            foreach (var brick in bricks)
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

