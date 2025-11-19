using DG.Tweening;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private TestEnemyScrip enemyScrip;
    private Tween tween;
    private void Start()
    {
        enemyScrip = GetComponentInParent<TestEnemyScrip>();

        tween = transform
  .DORotate(new Vector3(0, 0, 15), 0.5f, RotateMode.LocalAxisAdd)
  .SetLoops(-1, LoopType.Yoyo)
  .SetEase(Ease.InOutSine)
  .Pause();


    }
    void Update()
    {
        if (enemyScrip.Jobend == false)
        {
            tween.Pause();
        }
        if (enemyScrip.Jobend == true)
        {
            tween.Play();
        }
    }
}
