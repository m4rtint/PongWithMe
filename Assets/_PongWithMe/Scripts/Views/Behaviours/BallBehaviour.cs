using UnityEngine;

namespace PongWithMe
{
    public interface IBall
    {
        Vector3 GetPosition { get; }
        void SetPosition(Vector3 position);
        Direction LastHitFrom { get; }
    }
    
    public class BallBehaviour : MonoBehaviour, IBall
    {
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _minForce = 5f;
        [SerializeField] private Vector2 _startingForce = new Vector2(1f, 0.1f);
        [SerializeField] private TrailRenderer _renderer = null;
        
        private Rigidbody2D _rigidbody = null;
        private Direction _lastHitFrom = Direction.Left;
        private IStateManager _stateManager = null;
        private Vector3 _spawnPosition = Vector3.zero;
        
        public Direction LastHitFrom => _lastHitFrom;

        public Vector3 GetPosition => transform.position;
        
        public void Initialize(IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
            _stateManager.OnStateChanged += HandleStateChange;
        }

        public void CleanUp()
        {
            transform.position = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
        }

        public void Reset()
        {
            transform.position = _spawnPosition;
            _renderer.Clear();
            SetTrailColor(Color.white);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
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
                _rigidbody.AddForce(_startingForce);
            }
        }

        private void SetTrailColor(Color color)
        {
            _renderer.startColor = color;
            _renderer.endColor = Color.white;
        }
        #endregion
        
        #region Mono
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spawnPosition = transform.position;
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
                SetTrailColor(paddle.PaddleColor);
            }
        }
        #endregion
    }
}

