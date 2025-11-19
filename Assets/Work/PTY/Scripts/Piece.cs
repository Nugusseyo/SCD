using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Work.JYG.Code;
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

    [SerializeField] private SpriteRenderer[] energyBarUIList;
    private int[] _energyBarUISortingOrders;
    [SerializeField] private GameObject energyBar;

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
    }
    
    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;
        
        for(int i = 0; i < energyBarUIList.Length; i++)
            _energyBarUISortingOrders[i] = energyBarUIList[i].sortingOrder;
    }

    private void Update()
    {
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            TakeDamage(10, gameObject);
        }
    }

    public void OnHold(bool hold)
    {
        _collider.enabled = !_collider.enabled;
        if (hold)
        {
            _spriteRenderer.sortingOrder = 10;
            foreach (var s in energyBarUIList)
            {
                s.sortingOrder += 10;
            }
        }
        else
        {
            _spriteRenderer.sortingOrder = 0;
            int i = 0;
            foreach (var s in energyBarUIList)
            {
                s.sortingOrder = _energyBarUISortingOrders[i];
                i++;
            }
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
        if(energyBar == null) return;
        
        energyBar.transform.localScale = new Vector3((float)CurrentEnergy / MaxEnergy, energyBar.transform.localScale.y, energyBar.transform.localScale.z);
    }

    public int AttackDamage { get; set; }

    public void TakeDamage(int damage, GameObject attacker)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        
        Debug.Log($"{attacker} 이 {curCellPos} 에 있는 {gameObject.name} 에게 피해 {damage} 을/를 주었다!");
    }

    public void Die()
    {
        Destroy(gameObject);
        Debug.Log($"으앙 {curCellPos} 에 있는 {gameObject.name} 주금");
    }

    public int CurrentHealth { get; set; }

    public int MaxHealth => StatManager.Instance.ReturnPieceDamage[pieceData.pieceIndex];

    public bool IsDead { get; set; }
}