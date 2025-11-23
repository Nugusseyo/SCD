using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.JYG.Code;
using Work.JYG.Code.Chessboard.Pieces;
using Work.PTY.Scripts;
using YGPacks.PoolManager;

public class Piece : MonoBehaviour, ITurnAble, IAgentHealth, IPoolable
{
    public int MaxEnergy { get; set; } = 2;
    [field: SerializeField] public int CurrentEnergy { get; set; }
    public bool IsEnd { get; set; }

// ▽ 여기부터 수정
    public int AttackDamage
    {
        get
        {
            if (StatManager.Instance == null ||
                StatManager.Instance.ReturnPieceDamage == null ||
                pieceData == null ||
                pieceData.pieceIndex < 0 ||
                pieceData.pieceIndex >= StatManager.Instance.ReturnPieceDamage.Length)
                return 0;

            return StatManager.Instance.ReturnPieceDamage[pieceData.pieceIndex];
        }
    }

    public int CurrentHealth { get; set; }

    public int MaxHealth
    {
        get
        {
            if (StatManager.Instance == null ||
                StatManager.Instance.ReturnPieceHealth == null ||
                pieceData == null ||
                pieceData.pieceIndex < 0 ||
                pieceData.pieceIndex >= StatManager.Instance.ReturnPieceHealth.Length)
                return 1;

            return StatManager.Instance.ReturnPieceHealth[pieceData.pieceIndex];
        }
    }
    public bool IsDead { get; set; }

    public Action OnAttributeChanged;
    
    public string Name => "Piece";
    public GameObject GameObject => gameObject;
    
    public PieceSO pieceData;
    public List<ObjectVectorListSO> pieceVectorLists;
    [field:SerializeField] public List<AttributeSO> Attributes { get; set; }
    public List<AttributeSO> negativeAttributes;

    public Vector3Int curCellPos;

    public bool isSelected;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D _collider;

    [SerializeField] private SpriteRenderer[] uIList;
    private int[] _uISortingOrders;
    [SerializeField] private GameObject energyBar;
    [SerializeField] private GameObject healthBar;

    [SerializeField] private MatChange materialChange;

    public void AppearanceItem()
    {
        EventManager.Instance.AddList(this);
        Attributes.Clear();
    }

    public void ResetItem()
    {
        EventManager.Instance.RemoveList(this);
        if (Attributes != null)
            Attributes.Clear();

        OnAttributeChanged?.Invoke();
        
        if (materialChange != null)
        {
            try
            {
                materialChange.ResetColor();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[Piece] MatChange.ResetColor 예외 무시: {e.Message}");
            }
        }

        UpdateUI();
    }
    
    public void SetData()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && pieceData != null)
            spriteRenderer.sprite = pieceData.sprite;
    }
    
    private void Awake()
    {

        if (Attributes == null)
            Attributes = new List<AttributeSO>();

        if (pieceVectorLists == null)
            pieceVectorLists = new List<ObjectVectorListSO>();

        CurrentEnergy = MaxEnergy;

        _uISortingOrders = new int[uIList.Length];
        for (int i = 0; i < uIList.Length; i++)
            _uISortingOrders[i] = uIList[i].sortingOrder;

        OnAttributeChanged += AttributeChange;
    }


    private void AttributeChange()
    {
        if(Attributes.Count > 0)
            foreach (var a in Attributes)
                if (a.additionalVectorList != null)
                {
                    pieceVectorLists.Add(a.additionalVectorList);
                    pieceVectorLists = pieceVectorLists.Distinct().ToList();
                }
        
        ResetEnergy();
        UpdateUI();
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
            OnAttributeChanged?.Invoke();
        }
    }

    public void OnHold(bool hold)
    {
        if (_collider != null)
            _collider.enabled = !hold;

        if (spriteRenderer == null || uIList == null || uIList.Length == 0)
            return;

        
        if (_uISortingOrders == null || _uISortingOrders.Length != uIList.Length)
        {
            _uISortingOrders = new int[uIList.Length];
            for (int i = 0; i < uIList.Length; i++)
                _uISortingOrders[i] = uIList[i].sortingOrder;
        }

        if (hold)
        {
            spriteRenderer.sortingOrder = 10;
            foreach (var s in uIList)
                s.sortingOrder += 10;
        }
        else
        {
            spriteRenderer.sortingOrder = 0;

            // 🔹 둘 중 더 작은 길이만큼만 접근해서 IndexOutOfRange 방지
            int count = Mathf.Min(uIList.Length, _uISortingOrders.Length);
            for (int i = 0; i < count; i++)
                uIList[i].sortingOrder = _uISortingOrders[i];
        }
    }


    public void ReduceEnergy(int amount)
    {
        Debug.Log("에너지 소모");
        CurrentEnergy = Mathf.Clamp(CurrentEnergy - amount, 0, GetFinalMaxEnergy());
        UpdateUI();
    }

    public void ResetEnergy()
    {
        CurrentEnergy = GetFinalMaxEnergy();
    }
    
    public void UpdateUI()
    {
        Debug.Log("UI업뎃");
        if (energyBar == null || healthBar == null) return;
        if (pieceData == null) return;

        float maxEnergy = GetFinalMaxEnergy();
        float maxHealth = GetFinalMaxHealth();
        if (maxEnergy <= 0f || maxHealth <= 0f) return;

        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, (int)maxEnergy);
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, (int)maxHealth);

        energyBar.transform.localScale = new Vector3(
            (float)CurrentEnergy / maxEnergy,
            energyBar.transform.localScale.y,
            energyBar.transform.localScale.z);

        healthBar.transform.localScale = new Vector3(
            (float)CurrentHealth / maxHealth,
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z);
    }

    public void Heal(int amount, GameObject healer)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, GetFinalMaxHealth());
        UpdateUI();
        Debug.Log($"{healer.name} 이 {curCellPos} 에 있는 {gameObject.name} 을/를 {amount} 만큼 회복시켜 주었다!");
    }

    public int GetFinalDamage()
    {
        int attributeAdditionalDamage = 0;
        if (Attributes.Count > 0)
            foreach (var a in Attributes)
                if(a.dmgUpPercent != 0)
                    attributeAdditionalDamage += (int)(AttackDamage * ((float)a.dmgUpPercent / 100));

        return AttackDamage + attributeAdditionalDamage;
    }

    public int GetFinalMaxHealth()
    {
        int attributeAdditionalMaxHealth = 0;
        if (Attributes.Count > 0)
            foreach (var a in Attributes)
                if(a.hpUpPercent != 0)
                    attributeAdditionalMaxHealth += (int)(MaxHealth * ((float)a.hpUpPercent / 100));
        
        return MaxHealth + attributeAdditionalMaxHealth;
    }

    public int GetFinalMaxEnergy()
    {
        int additionalEnergy = 0;
        if(Attributes.Count > 0)
            foreach (var a in Attributes)
                additionalEnergy += a.energyUpAmount;
        
        return MaxEnergy + additionalEnergy;
    }
    
    public void TakeDamage(int damage, GameObject attacker)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, GetFinalMaxHealth());
        
        if (materialChange != null)
        {
            try
            {
                StartCoroutine(materialChange.ColorChange());
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[Piece] MatChange.ColorChange 예외 무시: {e.Message}");
            }
        }

        UpdateUI();

        if (CurrentHealth <= 0)
        {
            Die();
        }

        Debug.Log($"{(attacker != null ? attacker.name : "알 수 없음")} 이 {curCellPos} 에 있는 {gameObject.name} 에게 피해 {damage} 을/를 주었다!");
    }


    public void Die()
    {
        PlayerPrefs.SetInt("PieceDie", PlayerPrefs.GetInt("PieceDie") + 1);
        ChallengeManager.Instance.OnChallengeSwitchContacted?.Invoke();
        BoardManager.Instance.TileCompos[curCellPos].SetOccupie(null);
        PoolManager.Instance.Push(this);
        Debug.Log($"으앙 {curCellPos} 에 있는 {gameObject.name} 주금");
    }
}