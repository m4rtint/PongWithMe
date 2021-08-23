using System;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GoalBehaviour : MonoBehaviour
    {
        private const float COLOR_CHANGE_ANIMATION_DURATION = 0.5F;
        private IPaddle _player = null;
        private SpriteRenderer _renderer = null;

        public event Action<IPaddle> OnGoalHit;
        
        public void Set(IPaddle player)
        {
            _player = player;
            _player.OnIsActiveUpdated += HandleIsActiveUpdated;
            SetupStyle();
        }

        private void HandleIsActiveUpdated(bool active)
        {
            _renderer.DOColor(Color.clear, COLOR_CHANGE_ANIMATION_DURATION).OnComplete(() =>
            {
                transform.localScale = Vector3.zero;
            });
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
            if (other.collider.TryGetComponent<BallBehaviour>(out _))
            {
                OnGoalHit?.Invoke(_player);
            }
        }
    }
}

