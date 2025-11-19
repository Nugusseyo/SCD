using UnityEngine;

public class Boom : MonoBehaviour
{
    [SerializeField] private int speed;
    private EnemyAttack attack;
    private ShotEnemy shot;
    [SerializeField] private bool isDamageBig;
    public void Des()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("АјАн");
            var colPlayer = collision.gameObject.GetComponent<TestPlayerStat>();
            shot = GetComponentInParent<ShotEnemy>();
            attack = GetComponentInParent<EnemyAttack>();
            attack.RangedAttack(colPlayer, shot.infos.EnemyStat.attack);
        }
    }
}
