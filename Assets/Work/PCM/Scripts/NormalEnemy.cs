using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void EnemySpcAct()
    {
        attack.Attack(stat.attack);
    }
}
