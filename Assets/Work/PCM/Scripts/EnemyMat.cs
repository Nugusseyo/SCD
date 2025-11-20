using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyMat : MonoBehaviour
{
    readonly float floatValue;

    Material material;
    Enemy enemy;
    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponentInParent<Enemy>();
        material = spriteRenderer.material;
    }
    void Update()
    {
        if (enemy.Jobend == false)
        {
            material.SetFloat("_OuterOutlineFade", 1);
        }

        if (enemy.Jobend == true)
        {
            material.SetFloat("_OuterOutlineFade", 0);

        }
    }
    public void Heal()
    {
        material.SetColor("_AddColorColor", Color.green);
        StartCoroutine(ColorChange());
    }
    public IEnumerator ColorChange()
    {
        material.SetFloat("_AddColorFade", 1);
        yield return new WaitForSeconds(0.3f);
        material.SetFloat("_AddColorFade", 0);
    }
}
