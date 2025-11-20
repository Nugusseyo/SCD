using System.Collections;
using UnityEngine;
using Work.PTY.Scripts;

public class TestPlayerStat : MonoBehaviour,IAgentHealth
{
    [SerializeField]private AgentStatSO infos;

    private Material material;
    [field:SerializeField]public int CurrentHealth { get; set; }
    [field:SerializeField]public int MaxHealth { get; set; }
    public bool IsDead { get; set; }
    public int AttackDamage { get; set; }

    private void Start()
    {
        material = GetComponent<Material>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }
    public void Die()
    {
    }

    public void ReduceHealth(int damage)
    {
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        CurrentHealth -= damage;
        StartCoroutine(ColorChange());
    }
    public IEnumerator ColorChange()
    {
        material.SetFloat("_AddColorFade", 1);
        yield return new WaitForSeconds(0.3f);
        material.SetFloat("_AddColorFade", 0);
    }

    private void Awake()
    {
        Debug.Log("여기인가?");
        MaxHealth = infos.hp;
        CurrentHealth = MaxHealth;
    }

}
