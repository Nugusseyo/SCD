using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;

    public bool isSelected;

    private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        gameObject.name = pieceData.type.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer != null)
            spriteRenderer.sprite = pieceData.sprite;
    }

    private void Update()
    {
        if (isSelected)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        }
    }
}
