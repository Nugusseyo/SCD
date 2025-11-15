using UnityEngine;

public class EvnetRenderer : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private readonly int Explosion_Hash = Animator.StringToHash("EXPLOSION");

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void ExplosionMeteo()
    {
        Animator.SetBool(Explosion_Hash, true);
    }
    public void ResetParam()
    {
        Animator.SetBool(Explosion_Hash, false);
    }
}