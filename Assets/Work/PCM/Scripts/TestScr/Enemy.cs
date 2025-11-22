using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.PTY.Scripts;

public abstract class Enemy : MonoBehaviour, ITurnAble, IAgentHealth
{
    public Action OnEnemyAttack;
    public Action OnEnemyMove;

    //IEnemyAttackable
    //EnemyAttack

    public EnemysSO infos; // ���� �����ؼ� EnemySO�� �����ϱ� // ���ʹ� ���ݵ� SO �ȿ� �����ϱ�
    protected EnemyBrain brain;// ��� �ѵ� ������Ƽ�� ������൵ ��
    protected EnemyAttack attack; // ��� �ѵ� ������Ƽ�� ������൵ �� ���� ����
    protected EnemyMat material;

    public bool IsEnd { get; set; } = true; // ���Ŀ� Json���� ����
    public int MaxEnergy { get; set; }
    [field: SerializeField] public int CurrentEnergy { get; set; }
    [SerializeField] private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } set { value = currentHealth; } }
    [field: SerializeField] public int MaxHealth { get; set; }
    public bool IsDead { get; set; }
    private bool myturn = true;
    public int AttackDamage { get; set; }

    private SpriteRenderer mySprite;
    private Sprite temporary;

    public Grid grid;

    private int orDm;
    private int orEn;
    private int orCoin;


    protected List<Vector3Int> attackResult = new List<Vector3Int>();

    public int coin;
    private void Awake()
    {
        MaxHealth = infos.EnemyStat.hp;
        currentHealth = MaxHealth;
        AttackDamage = infos.EnemyStat.attack;
        orDm = AttackDamage;
        orEn = CurrentEnergy;
        orCoin = coin;
    brain = GetComponent<EnemyBrain>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        attack = GetComponentInChildren<EnemyAttack>(); //EnemyBrain, EnemyAttack�� ��ü�� ���� ���ʹ� �ȿ� GameObject�� �����
        // GetComponetnInChilderen���� ������ , ���� ����
        material = GetComponentInChildren<EnemyMat>();
        temporary = mySprite.sprite;

        OnEnemyAttack += HandleEnemyAttackEvent;
    }
    private void Start()
    {
        grid = FindAnyObjectByType<Grid>();
        MaxEnergy = infos.Energy;
        CurrentEnergy = MaxEnergy;
        coin = infos.EnemyStat.coin;
        Vector3Int cell = grid.WorldToCell(transform.position);
        cell.y = 7;
        transform.position = grid.GetCellCenterWorld(cell);
        Vector3Int v3int = grid.WorldToCell(transform.position);
        mySprite.sprite = temporary;
        mySprite.color = Color.white;
        BoardManager.Instance.TileCompos[v3int].SetOccupie(gameObject);
    }

    private void OnDestroy()
    {
        OnEnemyAttack -= HandleEnemyAttackEvent;
    }

    private void HandleEnemyAttackEvent()
    {
        attack.AOE(infos.EnemyStat.attack);
        //����Ʈ ��
    }

    private void Update()
    {
        //if (Keyboard.current.aKey.wasPressedThisFrame&&IsEnd == false)
        //{
        //    StartCoroutine(EnemyCortine());
        //    gameObject.transform.GetChild(0).DOScale(new Vector3(0.8f, 0.8f,1), 0.5f);
        //}
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            IsEnd = false;
        }
        if (CurrentEnergy <= 0 && attack.EnemyAttackend == true && myturn == true)
        {
            EnemySubAct();
            StopAllCoroutines();
            myturn = false;
            IsEnd = true;
            gameObject.transform.GetChild(0).DOScale(new Vector3(0.6f, 0.6f, 1), 0.5f);
            CurrentEnergy = MaxEnergy;
            Debug.Log(IsEnd);
        }
        if (EnemyTurnManager.Instance.turn % 20 == 0 && EnemyTurnManager.Instance.turn != 0)
        {
            if (!gameObject.CompareTag("Boss"))
            {
                AttackDamage = orDm * (EnemyTurnManager.Instance.turn / 20) + 1;
                 coin =orCoin * (EnemyTurnManager.Instance.turn / 20) + 1;
                 CurrentEnergy=orEn * (EnemyTurnManager.Instance.turn / 20) + 10;
            }
        }
    }
    public void EnemyNorAct()
    {
        attackResult = attack.AttackCheck(infos.EnemyAttack.VectorList); //���ݰ����� �� ����                                                                                 //var = �ְ� �� Ÿ������ �� �˾Ƽ� ������� c#�� ��������. ������ , �ٸ� �����ڰ� �б� ������ => �ذ�
        
        if (attackResult.Count <= 0)
        {
            brain.GetMove(infos.EnemyMove.VectorList, infos.EnemyAttack.VectorList); //������ �̵�
        }
        else
        {
            Vector3Int v3ints = grid.WorldToCell(transform.position);
            BoardManager.Instance.TileCompos[v3ints].SetOccupie(gameObject);
            EnemySpcAct(); //������ �ൿ���� ��ӹ޾Ƽ� 
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
                Debug.Log(v3ints);
                BoardManager.Instance.TileCompos[v3ints].SetOccupie(null);
                EnemyNorAct();
                
                CurrentEnergy--;

            }
        }
    }
    public abstract void EnemySpcAct();
    public virtual void EnemySubAct()
    { }

    public void ReduceHealth(int damage)
    {
        material.Heal();
        CurrentHealth += damage;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        material.StartCoroutine(material.ColorChange());
        Debug.Log(damage);
        currentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }

    }


    public void Die()
    {
        DOTween.Kill(transform, complete: false);

        EventManager.Instance.RemoveList(this);
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
