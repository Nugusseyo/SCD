using UnityEngine;

namespace YGPacks.PoolManager
{
    [CreateAssetMenu(fileName = "New YgPoolItem", menuName = "SO/Pool/YgPoolItem", order = 0)]
    public class YgPoolItemSO : ScriptableObject
    {
        [ReadOnly] public string itemName;
        public string parentName; //Use this if you want the object to have a parent.
        public GameObject prefab;
        public int count;

        private void OnValidate()
        {
            if (prefab == null) return;
            
            IPoolable poolable = prefab.GetComponent<IPoolable>();
            if (poolable == null)
            {
                Debug.LogError($"Prefab {prefab.name} is not YgPoolable!!");
                prefab = null;
                return;
            }
            itemName = poolable.Name.Trim();
            parentName = parentName.Trim();
        }
    }
}