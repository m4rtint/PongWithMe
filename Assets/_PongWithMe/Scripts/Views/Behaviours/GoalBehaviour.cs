using System;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GoalBehaviour : MonoBehaviour
    {
        private const float COLOR_CHANGE_ANIMATION_DURATION = 1.0F;
        private IPaddle _player = null;
        private SpriteRenderer _renderer = null;

        public event Action<IPaddle> OnGoalHit;
        
        public void Set(IPaddle player)
        {
            if (_player != null)
            {
                _player.OnIsActiveUpdated -= HandleIsActiveUpdated;
            }
            
            _player = player;
            _player.OnIsActiveUpdated += HandleIsActiveUpdated;
            SetupStyle();
        }

        private void HandleIsActiveUpdated(bool active)
        {
            if (!active)
            {
                _renderer.DOColor(Color.clear, COLOR_CHANGE_ANIMATION_DURATION)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        transform.localScale = Vector3.zero;
                    });
            }
        }

        private void SetupStyle()
        {
            transform.localScale = _player.IsActive ? Vector3.one : Vector3.zero;

            _renderer.DOColor(Color.clear, COLOR_CHANGE_ANIMATION_DURATION).SetUpdate(true).OnComplete(() =>
            {
                if (_player.IsActive)
                {
                    _renderer.DOColor(_player.PlayerColor, COLOR_CHANGE_ANIMATION_DURATION).SetUpdate(true);
                }
            });
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

