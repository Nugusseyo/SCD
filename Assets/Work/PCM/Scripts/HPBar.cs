using DG.Tweening;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private TestEnemyScrip enemy;
    private int oldHealth;
    private Vector3 localScale;
    private void Awake()
    {
        enemy = GetComponentInParent<TestEnemyScrip>();
    }

    private void Update()
    {
        if (enemy.CurrentHealth != oldHealth)
        {
            oldHealth = enemy.CurrentHealth; 
            
            localScale.x = (float)enemy.CurrentHealth / enemy.MaxHealth;
            transform.DOScaleX(localScale.x,0.5f);
        }
        

    }
}
