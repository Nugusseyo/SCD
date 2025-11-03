using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;

    public Vector3Int curCellPos;

    public bool isSelected;
    public bool isPassable;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void OnValidate()
    {
        if (pieceData != null)
            gameObject.name = pieceData.type.ToString();
        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer != null && pieceData != null)
            _spriteRenderer.sprite = pieceData.sprite;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }
    
    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;

        if (BoardManager.Instance.TileCompos.ContainsKey(curCellPos))
            BoardManager.Instance.TileCompos[curCellPos].SetOccupie(gameObject);
        else
            Debug.LogError($"Tile not found at {curCellPos} for {gameObject.name}");
    }

    public void OnHold()
    {
        _collider.enabled = !_collider.enabled;
        if (_spriteRenderer.sortingOrder == 0)
            _spriteRenderer.sortingOrder = 1;
        else
            _spriteRenderer.sortingOrder = 0;
    }
}