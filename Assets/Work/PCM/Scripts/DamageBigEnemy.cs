using UnityEngine;

public class DamageBigEnemy : Enemy
{
    [SerializeField]private GameObject Slash;
    public override void EnemySpcAct()
    {
        GameObject slash = Instantiate(Slash);
        
        slash.transform.position = new Vector2(transform.position.x,transform.position.y-0.2f);
        attack.FastEnemyAttack(infos.EnemyStat.attack);

    }
}
