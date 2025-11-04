using UnityEngine;
using YGPacks.PoolManager;

namespace Work.PTY.Scripts
{
    public class ParticlePool : MonoBehaviour, IPoolable
    {
        public string Name { get; }
        public GameObject GameObject { get; }
        
        public void AppearanceItem()
        {
            throw new System.NotImplementedException();
        }

        public void ResetItem()
        {
            throw new System.NotImplementedException();
        }
    }
}