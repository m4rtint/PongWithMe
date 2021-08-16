using UnityEngine;

namespace PongWithMe
{
    public class BallBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private float _force = 10f;
        private Rigidbody2D _rigidbody = null;

        public void Initialize()
        {
            _rigidbody.AddForce(Vector3.one * _force);    
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var normalizedDirection = _rigidbody.velocity.normalized;
            _rigidbody.velocity = normalizedDirection * _force;
        }
    }
}

