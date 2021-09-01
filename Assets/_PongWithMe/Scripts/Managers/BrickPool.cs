using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public class BrickPool : ObjectPool<BrickBehaviour>
    {
        private const int KEY = 213;
        private const int SPAWN_AMOUNT = 200;
        
        [SerializeField] private BrickBehaviour _prefab = null;

        private List<BrickBehaviour> _listOfBricks = new List<BrickBehaviour>();
        
        public void Initialize()
        {
            var list = new List<PoolingObject<BrickBehaviour>>();
            var pool = CreatePoolingObject(KEY, _prefab, SPAWN_AMOUNT);
            list.Add(pool);
            base.Initialize(list);
        }

        public void SpawnBrickWith(Brick brick)
        {
            var brickBehaviour = PopPooledObject(KEY);
            brickBehaviour.gameObject.SetActive(true);
            brickBehaviour.Initialize(brick);
            _listOfBricks.Add(brickBehaviour);
        }
        
        public void CleanUp()
        {
            foreach (var brickBehaviour in _listOfBricks)
            {
                Release(brickBehaviour, KEY);
            }
        }
    }
}

