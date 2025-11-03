using UnityEngine;

public class Tank : Enemy
{
    public override void EnemySpcAct()
    {
        Hp += 5;
    }
}
