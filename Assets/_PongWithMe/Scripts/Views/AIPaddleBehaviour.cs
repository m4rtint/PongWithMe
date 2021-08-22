using UnityEngine;

namespace PongWithMe 
{
    public class AIPaddleBehaviour : MonoBehaviour
    {
        private AIPaddle _paddle = null;
        private GameObject _ball = null;
        
        public void Initialize(AIPaddle paddle)
        {
            _paddle = paddle;
        }

        private void FixedUpdate()
        {
            _paddle.OnFixedUpdate(transform.position);
        }
    }
}
