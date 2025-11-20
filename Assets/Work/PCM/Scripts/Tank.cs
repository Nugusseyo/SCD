using UnityEngine;

public class Tank : Enemy
{
    public override void EnemySpcAct()
    {
        ReduceHealth(5);
    }
}
