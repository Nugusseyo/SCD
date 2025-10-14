using UnityEngine;

namespace YGPacks.PoolManager
{
    public interface IYgPoolable
    {
        string Name { get; }
        GameObject GameObject { get; }
        public void AppearanceItem();
        public void ResetItem();

    }
}
