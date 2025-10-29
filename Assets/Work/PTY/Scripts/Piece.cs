using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer _spriteRenderer;

    private void OnValidate()
    {
        if (pieceData != null)
            gameObject.name = pieceData.type.ToString();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
            _spriteRenderer.sprite = pieceData.sprite;
    }

    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;

        if (BoardManager.Instance.tileCompos.ContainsKey(curCellPos))
            BoardManager.Instance.tileCompos[curCellPos].SetOccupie(gameObject);
        else
            Debug.LogError($"Tile not found at {curCellPos} for {gameObject.name}");
    }
}