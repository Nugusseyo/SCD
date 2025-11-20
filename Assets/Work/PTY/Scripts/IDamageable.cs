using UnityEngine;

namespace Work.PTY.Scripts
{
    public interface IDamageable
    {
        public int AttackDamage { get; }
        
        public void TakeDamage(int damage, GameObject attacker);

        public void Die();
    }
}