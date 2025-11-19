using UnityEngine;
using YGPacks.PoolManager;

public class Lightning : MonoBehaviour, IPoolable
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    public Vector3 targetPos;
    public Vector3 spawnPos;

    public string Name => "Lightning";

    public GameObject GameObject => gameObject;


    public void AppearanceItem()
    {
        throw new System.NotImplementedException();
    }

    public void ResetItem()
    {
        throw new System.NotImplementedException();
    }
}
