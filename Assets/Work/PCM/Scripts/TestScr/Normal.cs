using System;
using UnityEngine;

public class Normal : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        OnEnemyAttack?.Invoke();
    }
}
