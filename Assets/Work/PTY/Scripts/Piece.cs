using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;
using Work.PTY.Scripts;

public class Piece : MonoBehaviour, ITurnAble, IAgentHealth
{
    public int MaxEnergy { get; set; } = 2;
    public int CurrentEnergy { get; set; }
    public bool IsEnd { get; set; }
    
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;
    public AttributeSO[] attributes;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    [SerializeField] private SpriteRenderer energyUIParent;
    [SerializeField] private GameObject energyUI;
    [SerializeField] private SpriteRenderer energyBarUI;
    [SerializeField] private SpriteRenderer energyUIBackground;

    private int energyUIParentOrder;
    private int energyBarUIOrder;
    private int energyUIBackgroundOrder;
    private IAgentHealth _agentHealthImplementation;

    public void SetData()
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
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        CurrentEnergy = MaxEnergy;
        
        energyUIParentOrder = energyUIParent.sortingOrder;
        energyBarUIOrder = energyBarUI.sortingOrder;
        energyUIBackgroundOrder = energyUIBackground.sortingOrder;
    }
    
    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;
    }

    public void OnHold(bool hold)
    {
        _collider.enabled = !_collider.enabled;
        if (hold)
        {
            _spriteRenderer.sortingOrder = 10;
            energyUIParent.sortingOrder = 10 + energyUIParentOrder;
            energyBarUI.sortingOrder = 10 + energyBarUIOrder;
            energyUIBackground.sortingOrder = 10 + energyUIBackgroundOrder;
        }
        else
        {
            _spriteRenderer.sortingOrder = 0;
            energyUIParent.sortingOrder = energyUIParentOrder;
            energyBarUI.sortingOrder = energyBarUIOrder;
            energyUIBackground.sortingOrder = energyUIBackgroundOrder;
        }
            
    }

    public void ReduceEnergy(int amount)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy - amount, 0, MaxEnergy);
        UpdateEnergyUI();
    }

    public void ResetEnergy()
    {
        CurrentEnergy = MaxEnergy;
    }
    
    public void UpdateEnergyUI()
    {
        if(energyUI == null) return;
        
        energyUI.transform.localScale = new Vector3((float)CurrentEnergy / MaxEnergy, energyUI.transform.localScale.y, energyUI.transform.localScale.z);
    }

    public int AttackDamage { get; set; }

    public void TakeDamage(int damage, GameObject attacker)
    {
        
    }

    public void Die()
    {

    }

    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }

    public bool IsDead { get; set; }

    public void ReduceHealth(int damage)
    {
        
    }
}