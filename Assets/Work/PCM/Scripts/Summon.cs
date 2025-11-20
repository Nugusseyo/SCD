using UnityEngine;
using UnityEngine.InputSystem;

public class Summon : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        attack.AOE(infos.EnemyStat.attack);
    }
}
