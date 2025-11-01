using UnityEngine;

namespace Work.PTY.Scripts
{
    public interface IAgentHealth : IDamageable
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public bool IsDead { get; set; }

        public void ReduceHealth(int damage);
    }
}