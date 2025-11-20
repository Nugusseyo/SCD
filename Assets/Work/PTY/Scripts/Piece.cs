using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Work.JYG.Code;
using Work.JYG.Code.Chessboard.Pieces;
using Work.PTY.Scripts;
using YGPacks.PoolManager;

public class Piece : MonoBehaviour, ITurnAble, IAgentHealth, IPoolable
{
    public int MaxEnergy { get; set; } = 2;
    public int CurrentEnergy { get; set; }
    public bool IsEnd { get; set; }

    public int AttackDamage => StatManager.Instance.ReturnPieceDamage[pieceData.pieceIndex];
    public int CurrentHealth { get; set; }
    public int MaxHealth => StatManager.Instance.ReturnPieceHealth[pieceData.pieceIndex];
    public bool IsDead { get; set; }

    public string Name => "Piece";
    public GameObject GameObject => gameObject;
    
    public PieceSO pieceData;
    public List<ObjectVectorListSO> pieceVectorLists;
    public AttributeSO[] attributes;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    [SerializeField] private SpriteRenderer[] energyBarUIList;
    private int[] _energyBarUISortingOrders;
    [SerializeField] private GameObject energyBar;

    public void AppearanceItem()
    {
        EventManager.Instance.AddList(this);
    }

    public void ResetItem()
    {
        EventManager.Instance.RemoveList(this);
    }
    
    public void SetData()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer != null && pieceData != null)
            _spriteRenderer.sprite = pieceData.sprite;
    }
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        
        CurrentEnergy = MaxEnergy;
        
        _energyBarUISortingOrders = new int[energyBarUIList.Length];
        for(int i = 0; i < energyBarUIList.Length; i++)
            _energyBarUISortingOrders[i] = energyBarUIList[i].sortingOrder;
    }
    
    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;
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

    public void Heal(int amount, GameObject healer)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount, 0, MaxEnergy);
        
        Debug.Log($"{healer.name} 이 {curCellPos} 에 있는 {gameObject.name} 을/를 {amount} 만큼 회복시켜 주었다!");
    }
    
    public void TakeDamage(int damage, GameObject attacker)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        
        Debug.Log($"{attacker.name} 이 {curCellPos} 에 있는 {gameObject.name} 에게 피해 {damage} 을/를 주었다!");
    }

    public void Die()
    {
        PoolManager.Instance.Push(this);
        Debug.Log($"으앙 {curCellPos} 에 있는 {gameObject.name} 주금");
    }
}