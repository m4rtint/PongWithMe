using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PongWithMe
{
    public class PaddleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 0.2f;

        private PlayerInput _input = null;
        private Vector2 _movement = Vector2.zero;

        
        public void Initialize()
        {
            _input.SwitchCurrentControlScheme("Player_WASD");
        }

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            if (_movement == Vector2.zero)
            {
                return;
            }

            MovePlatformBy(_movement);
        }

        private void MovePlatformBy(Vector2 movement)
        {
            transform.position += new Vector3(movement.x * _speed, movement.y * _speed, 0);
        }
        
    }
}

