using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.PTY.Scripts;

public abstract class Enemy : MonoBehaviour, ITurnAble, IAgentHealth
{
    public Action OnEnemyAttack;
    public Action OnEnemyMove;

    //IEnemyAttackable
    //EnemyAttack

    public EnemysSO infos; // 둘이 병합해서 EnemySO로 결합하기 // 에너미 성격도 SO 안에 결합하기
    protected EnemyBrain brain;// 얘네 둘도 프로퍼티로 만들어줘도 됨
    protected EnemyAttack attack; // 얘네 둘도 프로퍼티로 만들어줘도 됨 싫음 말고
    protected EnemyMat material;

    public bool IsEnd { get; set; } = true; // 이후에 Json으로 저장
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


    protected List<Vector3Int> attackResult = new List<Vector3Int>();
    private void Awake()
    {
        MaxHealth = infos.EnemyStat.hp;
        currentHealth = MaxHealth;
        AttackDamage = infos.EnemyStat.attack;
        brain = GetComponent<EnemyBrain>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        attack = GetComponentInChildren<EnemyAttack>(); //EnemyBrain, EnemyAttack은 객체로 만들어서 에너미 안에 GameObject로 만들기
        // GetComponetnInChilderen으로 들고오기 , 싫음 말고
        material = GetComponentInChildren<EnemyMat>();
        temporary = mySprite.sprite;

        OnEnemyAttack += HandleEnemyAttackEvent;
    }
    private void Start()
    {
        grid = FindAnyObjectByType<Grid>();
        MaxEnergy = infos.Energy;
        CurrentEnergy = MaxEnergy;
        Vector3Int v3int = grid.WorldToCell(transform.position);
        try
        {
            BoardManager.Instance.TileCompos[v3int].SetOccupie(gameObject);
        }
        catch
        {
            Vector3Int cell = grid.WorldToCell(transform.position);
            cell.y = 7;
            transform.position = grid.GetCellCenterWorld(cell);
            mySprite.sprite = temporary;
            mySprite.color = Color.white;

        }
    }

    private void OnDestroy()
    {
        OnEnemyAttack -= HandleEnemyAttackEvent;
    }

    private void HandleEnemyAttackEvent()
    {
        attack.AOE(infos.EnemyStat.attack);
        //이펙트 등
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
                MaxHealth *= (EnemyTurnManager.Instance.turn / 20) + 1;
                AttackDamage *= (EnemyTurnManager.Instance.turn / 20) + 1;
            }
        }
    }
    public void EnemyNorAct()
    {
        attackResult = attack.AttackCheck(infos.EnemyAttack.VectorList); //공격가능한 애 감지                                                                                 //var = 애가 뭔 타입인지 지 알아서 집어오고 c#이 설정해줌. 안좋음 , 다른 개발자가 읽기 불편함 => 해결
        if (attackResult.Count <= 0)
        {
            brain.GetMove(infos.EnemyMove.VectorList, infos.EnemyAttack.VectorList); //없으면 이동
        }
        else
        {
            EnemySpcAct(); //있으면 행동실행 상속받아서 
        }
    }

    public IEnumerator EnemyCortine()
    {
        while (CurrentEnergy > 0)
        {
            myturn = true;
            if (attack.EnemyAttackend == true && IsEnd == false)
            {
                EnemyNorAct();
                Vector3Int v3int = grid.WorldToCell(transform.position);
                BoardManager.Instance.TileCompos[v3int].SetOccupie(gameObject);
                CurrentEnergy--;

            }
            yield return new WaitForSeconds(0.3f);
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
        CurrentHealth -= damage;
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
            Debug.Log(gameObject);
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
