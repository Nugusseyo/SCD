using System;
using UnityEngine;

public class Normal : Enemy
{
    public override void EnemySpcAct()
    {
        OnEnemyAttack?.Invoke();
    }
}
