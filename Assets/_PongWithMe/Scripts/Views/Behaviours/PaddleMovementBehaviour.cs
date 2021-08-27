using UnityEngine;

namespace PongWithMe
{
    public class PaddleMovementBehaviour : MonoBehaviour
    {
        public const float MOVE_LIMIT = 3F;
        private const float ROTATION_AMOUNT = 20f;
        
        private float _speed = 0.2f;
        private IInput _playerInput = null;
        private Direction _direction;
        private float _rotationAngle = 0;

        private bool IsPlacementHorizontal => _direction == Direction.Left || _direction == Direction.Right;

        public void Initialize(IInput input, Direction direction, float speed = 0.2f)
        {            
            _rotationAngle = transform.localRotation.eulerAngles.z;
            _playerInput = input;
            _direction = direction;
            _speed = speed;
        }

        public void Set(Direction direction)
        {
            _direction = direction;
            //_rotationAngle -= 90;
        }

        private void Update()
        {
            if (StateManager.Instance.GetState() != State.Play)
            {
                return;
            }
            
            ResetRotation();
            if (IsPlacementHorizontal)
            {
                HandleVerticalMovement();
            }
            else
            {
                HandleHorizontalMovement();
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

            if (_playerInput.IsPressingDown())
            {
                RotateBodyPositive();
            } 
            else if (_playerInput.IsPressingUp())
            {
                RotateBodyNegative();
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
            
            if (_playerInput.IsPressingLeft())
            {
                RotateBodyPositive();
            } 
            else if (_playerInput.IsPressingRight())
            {
                RotateBodyNegative();
            }

            ClampVerticalMovementIfNeeded();
        }

        private void RotateBodyPositive()
        {
            transform.localRotation = Quaternion.Euler(0, 0, _rotationAngle + ROTATION_AMOUNT);
        }

        private void RotateBodyNegative()
        {
            transform.localRotation = Quaternion.Euler(0, 0, _rotationAngle - ROTATION_AMOUNT);
        }

        private void ResetRotation()
        {
            transform.localRotation = Quaternion.Euler(0, 0, _rotationAngle);
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