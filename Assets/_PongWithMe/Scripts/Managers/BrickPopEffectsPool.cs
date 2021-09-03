using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public interface IBrickPopEffect
    {
        BrickBreakEffectsBehaviour GetEffect();
    }
    
    public class BrickPopEffectsPool : ObjectPool<BrickBreakEffectsBehaviour>, IBrickPopEffect
    {
        private const int KEY = 123;
        [SerializeField] 
        private BrickBreakEffectsBehaviour _prefab = null;
        
        #region Singleton
        public static IBrickPopEffect Instance { get; private set; }

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            var list = new List<PoolingObject<BrickBreakEffectsBehaviour>>();
            var pool = CreatePoolingObject(KEY, _prefab, 10);
            list.Add(pool);
            Initialize(list);
        }

        public BrickBreakEffectsBehaviour GetEffect()
        {
            return InstantPopEnqueueObject(KEY);
        }
    }
}
