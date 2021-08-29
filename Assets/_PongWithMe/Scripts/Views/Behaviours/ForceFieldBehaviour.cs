using System;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class ForceFieldBehaviour : MonoBehaviour
    {
        private const float ANIMATE_FORCEFIELD_DURATION = 0.25F;
        private ForceField _forceField = null;

        public void Initialize(ForceField forceField = null)
        {
            _forceField = forceField ?? new ForceField();
            _forceField.OnIsActiveChanged += HandleOnActiveChanged;
        }

        private void Awake()
        {
            Initialize();
            _forceField.IsActive = false;
        }

        private void HandleOnActiveChanged(bool state)
        {
            if (state)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            transform.DOScale(Vector3.one, ANIMATE_FORCEFIELD_DURATION)
                .SetUpdate(true)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
            {
                StateManager.Instance.SetState(State.Play);
            });
        }

        private void Deactivate()
        {
            transform.DOScale(Vector3.zero, ANIMATE_FORCEFIELD_DURATION);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _forceField.DecrementLive();
        }

        private void OnDestroy()
        {
            if (_forceField != null)
            {
                _forceField.OnIsActiveChanged -= HandleOnActiveChanged;
                _forceField = null;
            }
        }
    }

    public class ForceField
    {
        private bool _isActive = false;
        private int _currentlives = 0;

        public event Action<bool> OnIsActiveChanged;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnIsActiveChanged?.Invoke(value);
            }
        }

        public void IncrementLivesBy(int lives)
        {
            _currentlives += lives;
            if (_currentlives > 0)
            {
                IsActive = true;
            }
        }

        public void DecrementLive()
        {
            _currentlives = Mathf.Max(0, _currentlives - 1);
            if (_currentlives <= 0)
            {
                IsActive = false;
            }
        }
    }
}