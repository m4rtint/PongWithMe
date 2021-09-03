using System;
using DG.Tweening;
using PerigonGames;
using UnityEngine;

namespace PongWithMe
{
    public class GoalBehaviour : MonoBehaviour
    {
        private const float COLOR_CHANGE_ANIMATION_DURATION = 1.0F;
        
        [SerializeField]
        private SpriteRenderer _renderer = null;
        [SerializeField] 
        private ForceFieldBehaviour _forceFieldBehaviour = null;
        
        private IPaddle _player = null;
        private ForceField _forceField = null;
        public event Action<IPaddle> OnGoalHit;

        public void Initialize()
        {
            _forceField = new ForceField();
            _forceFieldBehaviour.Initialize(_forceField);
        }

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
        
        public void ActivateForceField(int lives)
        {
            _forceField.IncrementLivesBy(lives);
        }

        public void Reset()
        {
            _forceField.Reset();
        }

        private void HandleIsActiveUpdated(bool active)
        {
            if (active)
            {
                ActivateGoal();
            }
            else
            {
                DeactivateGoal();
            }
        }

        private void ActivateGoal()
        {
            transform.ResetScale();
            _renderer.DOKill(true);
            _renderer.DOColor(_player.PlayerColor, COLOR_CHANGE_ANIMATION_DURATION)
                .SetUpdate(true);
        }

        private void DeactivateGoal()
        {
            _renderer.DOKill(true);
            _renderer.DOColor(Color.clear, COLOR_CHANGE_ANIMATION_DURATION)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.zero;
                });
        }

        private void SetupStyle()
        {
            transform.localScale = _player.IsActive ? Vector3.one : Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(_renderer.DOColor(Color.clear, COLOR_CHANGE_ANIMATION_DURATION)).SetUpdate(true);
            if (_player.IsActive)
            {
                sequence.Append(_renderer.DOColor(_player.PlayerColor, COLOR_CHANGE_ANIMATION_DURATION));
            }
        }
#region Mono
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent<BallBehaviour>(out _))
            {
                OnGoalHit?.Invoke(_player);
            }
        }
#endregion

    }
}

