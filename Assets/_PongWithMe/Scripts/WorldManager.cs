using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private BrickPool _brickPool = null;
        
        private void Start()
        {
            _brickPool.Initialize();
        }
    } 
}

