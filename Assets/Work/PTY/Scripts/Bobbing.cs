using DG.Tweening;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float height = 0.5f;
    public float duration = 1f;

    void Start()
    {
        Vector3 targetPos = transform.position + Vector3.up * height;
        transform.DOMove(targetPos, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
