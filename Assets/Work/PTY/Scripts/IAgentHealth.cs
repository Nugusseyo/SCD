namespace Work.PTY.Scripts
{
    public interface IAgentHealth : IDamageable
    {
        public int CurrentHealth { get; set; }

        public int MaxHealth { get; }
    }
}