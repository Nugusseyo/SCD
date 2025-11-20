using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void EnemySpcAct()
    {
        attack.AOE(infos.EnemyStat.attack);
    }
}
