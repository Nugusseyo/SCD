using System.Collections.Generic;
using UnityEngine;

namespace YGPacks.PoolManager
{
    [CreateAssetMenu(fileName = "New YgPoolList", menuName = "SO/Pool/YgPoolList", order = 0)]
    public class YgPoolListSO : ScriptableObject
    {
        public List<YgPoolItemSO> poolItemList;
    }
}