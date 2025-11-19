using UnityEngine;
using UnityEngine.InputSystem;
using YGPacks.PoolManager;

public class TestPoolObj : MonoBehaviour, IPoolable
{
    public string Name => gameObject.name;
    public GameObject GameObject => gameObject;
    public void AppearanceItem()
    {
        Debug.Log("나 왔띠");
    }

    public void ResetItem()
    {
        Debug.Log("리셋띠");
    }

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            PoolManager.Instance.Push(this);
        }
    }
}
