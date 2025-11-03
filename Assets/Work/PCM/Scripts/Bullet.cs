using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private int speed;
    private int z;
    private EnemyAttack attack;
    private ShotEnemy shot;
    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _rb.linearVelocityY = -(speed);
        transform.rotation = Quaternion.Euler(0, 0, z);
        z += 5;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("АјАн");
            var colPlayer = collision.gameObject.GetComponent<TestPlayerStat>();
            shot = GetComponentInParent<ShotEnemy>();
            attack = GetComponentInParent<EnemyAttack>();
            attack.RangedAttack(colPlayer,shot.stat.attack);
            Destroy(gameObject);
        }
    }
}
