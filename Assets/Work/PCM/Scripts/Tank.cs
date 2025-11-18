using UnityEngine;

public class Tank : TestEnemyScrip
{
    public override void EnemySpcAct()
    {
        CurrentHealth += 5;
    }
}
