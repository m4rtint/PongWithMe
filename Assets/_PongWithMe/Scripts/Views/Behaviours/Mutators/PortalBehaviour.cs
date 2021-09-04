using System;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class PortalBehaviour : MonoBehaviour
    {
        private const float SHOW_PORTAL_ANIMATION_DURATION = 0.5f;
        private Portal _portal = null;

        public void Initialize(Portal portal)
        {
            _portal = portal;
            _portal.OnIsActiveChanged += HandleOnActiveChange;
        }

        private void HandleOnActiveChange(bool state)
        {
            if (state)
            {
                ShowPortal();
            }
            else
            {
                HidePortal();
            }
        }

        private void ShowPortal()
        {
            transform.DOScale(Vector3.one, SHOW_PORTAL_ANIMATION_DURATION).SetEase(Ease.InQuart).SetUpdate(true).OnComplete(() =>
            {
                StateManager.Instance.SetState(State.Play);
            });
        }
        
        private void HidePortal()
        {
            transform.DOScale(Vector3.zero, SHOW_PORTAL_ANIMATION_DURATION).SetEase(Ease.InCubic);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<BallBehaviour>(out var ball))
            {
                _portal.BallEnteredPortal(ball);
            }
        }

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_portal != null)
            {
                _portal.OnIsActiveChanged -= HandleOnActiveChange;
            }
        }
    }
}
