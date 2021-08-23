using System;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GoalBehaviour : MonoBehaviour
    {
        private IPaddle _player = null;
        private SpriteRenderer _renderer = null;
        
        public void Set(IPaddle player)
        {
            _player = player;
            SetupStyle();
        }

        private void SetupStyle()
        {
            _renderer.color = _player.PlayerColor;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent<BallBehaviour>(out var ball))
            {
                // Minus health   
            }
        }
    }
}

