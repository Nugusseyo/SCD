using DG.Tweening;
using UnityEngine;

public class Visualswing : MonoBehaviour
{
    private TestEnemyScrip enemyScrip;

    private void Start()
    {
        enemyScrip = GetComponent<TestEnemyScrip>();
    }
    void Update()
    {
        if (enemyScrip.Jobend == true)
        {
            transform.DORotate(new Vector3(0, 0, 5), 0.5f);
            transform.DORotate(new Vector3(0, 0, -5), 0.5f);
        }
    }
}
