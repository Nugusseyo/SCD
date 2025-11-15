using UnityEngine;

public class ShotEnemy : TestEnemyScrip
{
    [SerializeField]private GameObject bullet;
    public override void EnemySpcAct()
    {
        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.position = transform.position;
        bullet.transform.SetParent(transform);
    }
}
