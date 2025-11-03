using Unity.Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource source;
    [SerializeField] private float power;

    [ContextMenu("Shake")]
    public void ShakeCamera()
    {
        source.GenerateImpulse(power);
    }
}
