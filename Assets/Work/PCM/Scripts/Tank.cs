using UnityEngine;

public class Tank : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        ReduceHealth(5);
    }
}
