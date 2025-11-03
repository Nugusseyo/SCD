using System;
using UnityEngine;

public interface IEnemyAttackable
{
    public event Action OnEnemyAttack;
    public EnemyAttack EnemyAttack { get; }
}
