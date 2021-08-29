using UnityEngine;

namespace PongWithMe
{
    public interface IBall
    {
        Vector3 GetPosition { get; }    
        Direction LastHitFrom { get; }
    }
    
    public class BallBehaviour : MonoBehaviour, IBall
    {
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _minForce = 5f;
        [SerializeField] private TrailRenderer _renderer = null;
        
        private Rigidbody2D _rigidbody = null;
        private Direction _lastHitFrom = Direction.Left;

        public Direction LastHitFrom => _lastHitFrom;

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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<PaddleBehaviour>(out var paddle))
            {
                _lastHitFrom = paddle.PaddleDirection;
                _renderer.startColor = paddle.PaddleColor;
                _renderer.endColor = Color.white;
            }
        }
    }
}

