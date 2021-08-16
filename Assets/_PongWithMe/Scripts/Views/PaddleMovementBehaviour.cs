using UnityEngine;

namespace PongWithMe
{
    public class PaddleMovementBehaviour : MonoBehaviour
    {
        private const float MOVE_LIMIT = 3F;
        private float _speed = 0.2f;
        private IInput _playerInput = null;
        private Direction _direction;

        private bool IsHorizontal => _direction == Direction.Left || _direction == Direction.Right;

        public void Initialize(IInput input, Direction direction, float speed = 0.2f)
        {
            _playerInput = input;
            _direction = direction;
            _speed = speed;
        }

        private void Update()
        {
            if (IsHorizontal)
            {
                HandleHorizontalMovement();
            }
            else
            {
                HandleVerticalMovement();
            }
        }

        private void HandleHorizontalMovement()
        {
            if (_playerInput.IsPressingLeft())
            {
                MovePositionBy(Vector3.left);
            } 
            
            if (_playerInput.IsPressingRight())
            {
                MovePositionBy(Vector3.right);
            }

            ClampHorizontalMovementIfNeeded();
        }
        
        private void HandleVerticalMovement()
        {
            if (_playerInput.IsPressingUp())
            {
                MovePositionBy(Vector3.up);
            } 
            
            if (_playerInput.IsPressingDown())
            {
                MovePositionBy(Vector3.down);
            }

            ClampVerticalMovementIfNeeded();
        }

        private void MovePositionBy(Vector3 moveAmount)
        {
            transform.position += moveAmount * _speed;
        }

        private void ClampHorizontalMovementIfNeeded()
        {
            var position = transform.position;
            position.x = Mathf.Clamp(position.x, -MOVE_LIMIT, MOVE_LIMIT);
            transform.position = position;
        }
        
        private void ClampVerticalMovementIfNeeded()
        {
            var position = transform.position;
            position.y = Mathf.Clamp(position.y, -MOVE_LIMIT, MOVE_LIMIT);
            transform.position = position;
        }
    }
}