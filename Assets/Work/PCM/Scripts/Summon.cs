using UnityEngine;
using UnityEngine.InputSystem;

public class Summon : Enemy
{
    public override void EnemySpcAct()
    {
        attack.AOE(infos.EnemyStat.attack);
    }
}
