using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject occupiePiece;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetOccupie(GameObject occupie)
    {
        occupiePiece = occupie;
    }
    
    public GameObject CheckTile()
    {
        return occupiePiece;
    }

    public void ToggleSpriteRenderer()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
    
}
