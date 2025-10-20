using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;

    private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        gameObject.name = pieceData.type.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer != null)
            spriteRenderer.sprite = pieceData.sprite;
    }
}
