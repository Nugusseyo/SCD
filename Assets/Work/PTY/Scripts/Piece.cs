using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class Piece : MonoBehaviour, ITurnAble
{
    public int MaxEnergy { get; set; } = 2;
    public int CurrentEnergy { get; set; }
    public bool IsEnd { get; set; }
    
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    
    [SerializeField] private GameObject energyUI;

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
        CurrentEnergy = MaxEnergy;
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

    public void ReduceEnergy(int amount)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy - amount, 0, MaxEnergy);
        UpdateEnergyUI();
    }
    
    public void UpdateEnergyUI()
    {
        energyUI.transform.localScale = new Vector3((float)CurrentEnergy / MaxEnergy, energyUI.transform.localScale.y, energyUI.transform.localScale.z);
    }
}