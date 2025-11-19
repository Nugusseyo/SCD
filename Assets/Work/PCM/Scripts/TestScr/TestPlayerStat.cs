using UnityEngine;
using Work.PTY.Scripts;

public class TestPlayerStat : MonoBehaviour,IAgentHealth
{
    [SerializeField]private AgentStatSO infos;

    [field:SerializeField]public int CurrentHealth { get; set; }
    [field:SerializeField]public int MaxHealth { get; set; }
    public bool IsDead { get; set; }
    public int AttackDamage { get; set; }

    public void Die()
    {
    }

    public void ReduceHealth(int damage)
    {
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        CurrentHealth -= damage;
    }

    private void Awake()
    {
        Debug.Log("여기인가?");
        MaxHealth = infos.hp;
        CurrentHealth = MaxHealth;
    }

}
