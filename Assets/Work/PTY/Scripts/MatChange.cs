using System.Collections;
using UnityEngine;

public class MatChange : MonoBehaviour
{
    private Material material;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }
    
    public IEnumerator ColorChange()
    {
        material.SetFloat("_AddColorFade", 1);
        yield return new WaitForSeconds(0.3f);
        material.SetFloat("_AddColorFade", 0);
    }
}
