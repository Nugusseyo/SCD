using UnityEngine;
using UnityEngine.Events;
public class MeteoRenderer : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private static readonly int Explosion_Hash = Animator.StringToHash("EXPLOSION");

    public UnityEvent ResetMeteo;

    public UnityEvent ExplosionMt;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void ExplosionMeteo()
    {
        Animator.SetBool(Explosion_Hash, true);
        ExplosionMt?.Invoke();
    }
    public void ResetParam()
    {
        Animator.SetBool(Explosion_Hash, false);
    }
    public void ResetMeteor()
    {
        ResetMeteo?.Invoke();
    }
}