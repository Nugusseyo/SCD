using csiimnida.CSILib.SoundManager.RunTime;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.JYG.Code;
using Work.PTY.Scripts;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public abstract class Enemy : MonoBehaviour, ITurnAble, IAgentHealth
{
    public Action OnEnemyAttack;
    public Action OnEnemyMove;

    //IEnemyAttackable
    //EnemyAttack

    public EnemysSO infos;                // 적 정보 SO
    protected EnemyBrain brain;           // 이동 로직
    protected EnemyAttack attack;         // 공격 로직
    protected EnemyMat material;

    public bool IsEnd { get; set; } = true;
    public int MaxEnergy { get; set; }
    [field: SerializeField] public int CurrentEnergy { get; set; }

    [SerializeField] private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    [field: SerializeField] public int MaxHealth { get; set; }
    public bool IsDead { get; set; }
    private bool myturn = true;
    public int AttackDamage { get; set; }

    private SpriteRenderer mySprite;
    private Sprite temporary;

    public Grid grid;

    private bool statUp;

    protected List<Vector3Int> attackResult = new List<Vector3Int>();

    public int coin;

    private void Awake()
    {
        if (EventManager.Instance.GameTurn >= 20)
        {
            AttackDamage = infos.EnemyStat.attack * (int)(1f + 0.5 * (EnemyTurnManager.Instance.turn / 20));
            MaxHealth = infos.EnemyStat.hp * (int)(1f+0.5*(EnemyTurnManager.Instance.turn / 20));
        }
        else
        {
            AttackDamage = infos.EnemyStat.attack;
            MaxHealth = infos.EnemyStat.hp;
        }

        currentHealth = MaxHealth;
        MaxEnergy = infos.Energy;
        coin = infos.EnemyStat.coin;
        CurrentEnergy = MaxEnergy;

        brain = GetComponent<EnemyBrain>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        attack = GetComponentInChildren<EnemyAttack>();   // EnemyBrain, EnemyAttack는 자식에 붙음
        material = GetComponentInChildren<EnemyMat>();
        temporary = mySprite != null ? mySprite.sprite : null;

        OnEnemyAttack += HandleEnemyAttackEvent;
    }

    [NonSerialized] public bool LoadedFromSave = false;   // 🔹 세이브에서 로드된 애인지 여부

    private void Start()
    {
        // 그리드 세팅
        if (grid == null)
        {
            if (BoardManager.Instance != null)
                grid = BoardManager.Instance.boardTileGrid;
            if (grid == null)
                grid = FindAnyObjectByType<Grid>();
        } 
        if (grid != null)
        {
            // 🔹 세이브에서 로드된 경우에는 위치를 건드리지 않음
            if (!LoadedFromSave)
            {
                Vector3Int cell = grid.WorldToCell(transform.position);
                cell.y = 7;
                transform.position = grid.GetCellCenterWorld(cell);
            }

            Vector3Int v3int = grid.WorldToCell(transform.position);

            if (mySprite != null)
            {
                mySprite.sprite = temporary;
                mySprite.color = Color.white;
            }

            // 타일 점유
            if (BoardManager.Instance != null &&
                BoardManager.Instance.TileCompos.TryGetValue(v3int, out Tile tile))
            {
                tile.SetOccupie(gameObject);
            }
        }

        // EventManager의 testEnemyList에 자신 등록 (중복 방지)
        if (EventManager.Instance != null)
        {
            if (!EventManager.Instance.testEnemyList.Contains(this))
                EventManager.Instance.AddList(this);
        }
    }

    private void OnDestroy()
    {
        OnEnemyAttack -= HandleEnemyAttackEvent;
    }

    private void HandleEnemyAttackEvent()
    {
        attack.AOE(infos.EnemyStat.attack);
        // 이펙트 등
    }

    private void Update()
    {
        // 디버그용 키
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            IsEnd = false;
        }   

        if (CurrentEnergy <= 0 && attack.EnemyAttackend == true && myturn == true)
        {
            
            StopAllCoroutines();
            myturn = false;
            IsEnd = true;
            gameObject.transform.GetChild(0).DOScale(new Vector3(0.6f, 0.6f, 1), 0.5f);
            CurrentEnergy = MaxEnergy;
            EnemySubAct();
        }
    }

    public void EnemyNorAct()
    {
        // 공격 가능한 타일 검사
        attackResult = attack.AttackCheck(infos.EnemyAttack.VectorList);
        if (attackResult.Count <= 0)
        {
            // 이동
            brain.GetMove(infos.EnemyMove.VectorList, infos.EnemyAttack.VectorList);
        }
        else
        {
            Vector3Int v3ints = grid.WorldToCell(transform.position);
            BoardManager.Instance.TileCompos[v3ints].SetOccupie(gameObject);
            EnemySpcAct();
        }
    }

    public IEnumerator EnemyCortine()
    {
        while (CurrentEnergy > 0)
        {
            yield return new WaitForSeconds(1f);
            myturn = true;
            if (attack.EnemyAttackend == true && IsEnd == false)
            {
                Vector3Int v3ints = grid.WorldToCell(transform.position);
                BoardManager.Instance.TileCompos[v3ints].SetOccupie(null);
                EnemyNorAct();

                CurrentEnergy--;
            }
        }
    }

    public abstract void EnemySpcAct();
    public virtual void EnemySubAct() { }

    public void ReduceHealth(int damage)
    {
        material.Heal();
        CurrentHealth += damage;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        material.StartCoroutine(material.ColorChange());
        currentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            CoinManager.Instance.AddCoins(infos.EnemyStat.coin);
            Die();
        }   
    }

    public virtual void Die()
    {
        if (gameObject.CompareTag("Boss"))
        {
            EnemyTurnManager.Instance.Bosstlist.Remove(gameObject);
        }

        DOTween.Kill(transform, complete: false);
        SoundManager.Instance.PlaySound("EnemyDie");

        if (EventManager.Instance != null)
            EventManager.Instance.RemoveList(this);

        PlayerPrefs.SetInt("EnemyDie", PlayerPrefs.GetInt("EnemyDie", 0) + 1);
        ChallengeManager.Instance.OnChallengeSwitchContacted?.Invoke();

        // 타일 점유 해제
        if (grid == null)
        {
            if (BoardManager.Instance != null)
                grid = BoardManager.Instance.boardTileGrid;
            if (grid == null)
                grid = FindAnyObjectByType<Grid>();
        }

        if (grid != null && BoardManager.Instance != null)
        {
            Vector3Int cell = grid.WorldToCell(transform.position);
            if (BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile))
                tile.SetOccupie(null);
        }
        Destroy(gameObject);
    }

    public void EnemyRealSpawn()
    {
        if (enabled == false)
        {
            gameObject.GetComponent<EnemySpawn>().SpawnTime();
        }
        else if (!IsEnd)
        {
            Coroutine c = StartCoroutine(EnemyCortine());
            transform.GetChild(0)
                .DOScale(new Vector3(0.8f, 0.8f, 1), 0.5f);
        }
    }
}
