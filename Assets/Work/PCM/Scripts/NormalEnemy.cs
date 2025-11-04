using UnityEngine;

public class NormalEnemy : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        attack.AOE(infos.EnemyStat.attack);
    }
}
