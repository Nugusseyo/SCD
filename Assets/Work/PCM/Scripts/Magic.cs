using UnityEngine;

public class Magic : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private int speed;
    private int z;
    private EnemyAttack attack;
    private SpawnsEnemy magic;
    [SerializeField] private bool isDamageBig;
    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _rb.linearVelocityY = -(speed);
        if (!isDamageBig)
        {
            transform.rotation = Quaternion.Euler(0, 0, z);
            z += 5;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var colPlayer = collision.gameObject.GetComponent<Piece>();
            magic = GetComponentInParent<SpawnsEnemy>();
            attack = GetComponent<EnemyAttack>();
            attack.RangedAttack(colPlayer, magic.infos.EnemyStat.attack);
            Destroy(gameObject);
        }
    }
}
