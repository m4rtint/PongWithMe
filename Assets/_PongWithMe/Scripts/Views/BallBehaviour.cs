using UnityEngine;

namespace PongWithMe
{
    public interface IBall
    {
        Vector3 GetPosition { get; }    
    }
    
    public class BallBehaviour : MonoBehaviour, IBall
    {
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _minForce = 5f;
        private Rigidbody2D _rigidbody = null;
        
        public Vector3 GetPosition => transform.position;
        
        public void Initialize()
        {
            _rigidbody.AddForce(Vector3.one * _minForce);    
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity;
            var normalizedDirection = velocity.normalized;
            var speed = velocity.magnitude;
            var clampedSpeed = Mathf.Clamp(speed, _minForce, _maxForce);
            _rigidbody.velocity = normalizedDirection * clampedSpeed;
        }
    }
}

