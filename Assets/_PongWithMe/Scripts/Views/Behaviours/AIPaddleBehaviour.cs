using UnityEngine;

namespace PongWithMe 
{
    public class AIPaddleBehaviour : MonoBehaviour
    {
        private AIPaddle _paddle = null;
        
        public void Initialize(AIPaddle paddle)
        {
            _paddle = paddle;
        }

        private void Update()
        {
            _paddle.OnUpdate(transform.position);
        }
    }
}
