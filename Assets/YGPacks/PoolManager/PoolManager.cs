using System.Collections.Generic;
using UnityEngine;
using YGPacks;
using YGPacks.PoolManager;

public class PoolManager : YgSingleton<PoolManager>
{
    [SerializeField] private YgPoolListSO poolList;
    
    private Dictionary<string, YgPool> _poolDictionary = new Dictionary<string, YgPool>();
    private Dictionary<string, GameObject> _parentDictionary = new Dictionary<string, GameObject>();
    

    protected override void Awake()
    {
        base.Awake();
        foreach (YgPoolItemSO pool in poolList.poolItemList)
        {
            Transform parent = transform;
            if (!string.IsNullOrEmpty(pool.parentName)) //Parent Name searching Null or Empty
            {
                if (_parentDictionary.TryGetValue(pool.parentName, out GameObject value))
                {
                    parent = value.transform;
                }
                else
                {
                    GameObject newParent = new GameObject();
                    newParent.transform.SetParent(transform);
                    newParent.name = pool.parentName;
                    _parentDictionary.Add(pool.parentName, newParent);
                    parent = newParent.transform;
                }
            }

            CreatePool(pool.prefab, pool.count, parent);
        }
    }

    private void CreatePool(GameObject poolItem, int count, Transform parent)
    {
        IYgPoolable poolable = poolItem.GetComponent<IYgPoolable>();
        if (poolable == null)
        {
            Debug.LogError($"Pool Item {poolItem.name} is not IYGPoolable");
            return;
        }

        YgPool pool = new YgPool(poolable, poolItem, count, parent);
        _poolDictionary.Add(poolItem.name, pool);
    }

    public IYgPoolable PopByName(string objName)
    {
        if (_poolDictionary.ContainsKey(objName))
        {
            IYgPoolable takeItem = _poolDictionary[objName].Pop();
            takeItem.ResetItem();
            takeItem.AppearanceItem();
            return takeItem;
        }
        Debug.LogError($"Item <{objName}> not found in PoolManager");
        return null;
    }

    public IYgPoolable PopByPoolItemSO(YgPoolItemSO poolItemSO)
    {
        if (poolItemSO == null)
        {
            Debug.LogError($"THERE IS NO PoolItemSO");
            return null;
        }

        if (_poolDictionary.ContainsKey(poolItemSO.name))
        {
            IYgPoolable takeItem = _poolDictionary[poolItemSO.name].Pop();
            takeItem.ResetItem();
            takeItem.AppearanceItem();
            return takeItem;
        }
        Debug.LogError($"Item <{poolItemSO.name}> not found in PoolManager Dictionary");
        return null;
    }

    public void Push(IYgPoolable returnItem)
    {
        if (_poolDictionary.ContainsKey(returnItem.Name))
        {
            _poolDictionary[returnItem.Name].Push(returnItem);
            return;
        }
        Debug.LogError($"Item {returnItem.Name} not found in PoolManager Dictionary");
    }
}
