using System;
using UnityEngine;

public class RealTestEnemy : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        OnEnemyAttack?.Invoke();
    }
}
