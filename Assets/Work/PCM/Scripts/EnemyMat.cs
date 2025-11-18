using UnityEngine;

public class EnemyMat : MonoBehaviour
{


    readonly float floatValue;

    Material material;
    TestEnemyScrip enemy;
    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponentInParent<TestEnemyScrip>();
        material = spriteRenderer.material;
    }
    void Update()
    {
        if (enemy.Job == true)
        {
            material.SetFloat("_OuterOutlineFade", 1);
        }

        if (enemy.Job == false)
        {
            material.SetFloat("_OuterOutlineFade", 0);
        }
    }
}
