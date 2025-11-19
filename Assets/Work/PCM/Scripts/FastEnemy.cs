using UnityEngine;

public class FastEnemy : TestEnemyScrip
{
    [SerializeField] private GameObject Slash;
    public override void EnemySpcAct()
    {
        for(int i = 0; i < attackResult.Count; i++) 
        {
            GameObject slash = Instantiate(Slash);
            slash.transform.position =grid.CellToWorld(attackResult[i]);
            OnEnemyAttack?.Invoke();

        }
    }
}
