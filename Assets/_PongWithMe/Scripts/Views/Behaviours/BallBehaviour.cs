using System;
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
        private IStateManager _stateManager = null;

        public Direction LastHitFrom => _lastHitFrom;

        public Vector3 GetPosition => transform.position;
        
        public void Initialize(IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            _stateManager.OnStateChanged += HandleStateChange;
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity;
            var normalizedDirection = velocity.normalized;
            var speed = velocity.magnitude;
            var clampedSpeed = Mathf.Clamp(speed, _minForce, _maxForce);
            _rigidbody.velocity = normalizedDirection * clampedSpeed;
        }
        #region Delegate

        private void HandleStateChange(State state)
        {
            if (state == State.StartGame)
            {
                _rigidbody.AddForce(Vector3.one * _minForce);
            }
        }
        #endregion
        
        #region Mono
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_stateManager.GetState() == State.Play)
            {
                ClampVelocity();
            }
        }

        private void OnDestroy()
        {
            _stateManager.OnStateChanged -= HandleStateChange;
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
        #endregion
    }
}

