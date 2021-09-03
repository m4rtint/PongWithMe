using UnityEngine;

namespace PongWithMe
{
    public class BrickBreakEffectsBehaviour : MonoBehaviour
    {
        private ParticleSystem _particleSystem = null;

        public void SetColor(Color color)
        {
            var main = _particleSystem.main;
            main.startColor = color;
        }

        public void Play()
        {
            _particleSystem.Play();
        }
        
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
}
