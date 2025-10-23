using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        gameObject.name = pieceData.type.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = pieceData.sprite;
    }

    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;
    }
}