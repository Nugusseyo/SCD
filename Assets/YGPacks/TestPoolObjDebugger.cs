using UnityEngine;
using UnityEngine.InputSystem;
using YGPacks.PoolManager;

public class TestPoolObjDebugger : MonoBehaviour
{
    [SerializeField] private YgPoolItemSO itemSO;
    [SerializeField] private GameObject prefab;
    void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            PoolManager.Instance.PopByName(prefab.name);
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            PoolManager.Instance.PopByPoolItemSO(itemSO);
        }
    }
}
