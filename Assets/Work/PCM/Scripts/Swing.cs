using DG.Tweening;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private Enemy enemyScrip;
    private Tween tween;
    private void Start()
    {
        enemyScrip = GetComponentInParent<Enemy>();

        tween = transform
        .DORotate(new Vector3(0, 0, 8), 0.8f, RotateMode.LocalAxisAdd)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine)
        .Pause();


    }
    void Update()
    {
        if (enemyScrip.Jobend == false)
        {
            tween.Pause();
            tween.Goto(0, true);
            transform.localRotation = Quaternion.identity;
        }
        if (enemyScrip.Jobend == true)
        {
            tween.Play();

        }
    }
}
