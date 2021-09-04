using System;
using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public class TeleporterBehaviour : MonoBehaviour
    {
        private int _index = 0;
        
        public event Action<BallBehaviour, int> OnTeleportStart;

        public bool IsActive = true;

        public void Initialize(int index)
        {
            _index = index;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsActive)
            {
                return;
            }
            if (other.TryGetComponent<BallBehaviour>(out var ball))
            {
                OnTeleportStart?.Invoke(ball, _index);
            }
        }
    }
}
