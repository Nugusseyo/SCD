using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.PTY.Scripts;

public abstract class TestEnemyScrip : MonoBehaviour, ITurnAble, IAgentHealth
{
    public Action OnEnemyAttack;
    public Action OnEnemyMove;

    //IEnemyAttackable
    //EnemyAttack

    public EnemysSO infos; // 둘이 병합해서 EnemySO로 결합하기 // 에너미 성격도 SO 안에 결합하기
    protected EnemyBrain brain;// 얘네 둘도 프로퍼티로 만들어줘도 됨
    protected EnemyAttack attack; // 얘네 둘도 프로퍼티로 만들어줘도 됨 싫음 말고
    [field: SerializeField] public bool Job { get; set; } = false;

    public bool IsEnd { get; set; } = false; // 이후에 Json으로 저장
    public int MaxEnergy { get; set; }
    [field: SerializeField] public int CurrentEnergy { get; set; }
    [SerializeField]private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } set {currentHealth= Mathf.Clamp(value,0,MaxHealth); }}
    [field:SerializeField]public int MaxHealth { get; set; }
    public bool IsDead { get ; set; }
    private bool myturn = true;

    public int Attack { get; set; }

    private Grid grid;
    private void Awake()
    {
        MaxHealth = infos.EnemyStat.hp;
        currentHealth = MaxHealth;
        Attack = infos.EnemyStat.attack;
        brain = GetComponent<EnemyBrain>();
        attack = GetComponentInChildren<EnemyAttack>(); //EnemyBrain, EnemyAttack은 객체로 만들어서 에너미 안에 GameObject로 만들기
        // GetComponetnInChilderen으로 들고오기 , 싫음 말고

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
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            StartCoroutine(EnemyCortine());
        }
        if (CurrentEnergy <= 0&&attack.EnemyAttackend == true&&myturn == true)
        {
            myturn = false;
            Job = false;
            CurrentEnergy = MaxEnergy;
        }

    }
    public void EnemyNorAct()
    {
        List<Vector3Int> attackReult = attack.AttackCheck(infos.EnemyAttack.VectorList); //공격가능한 애 감지                                                                                 //var = 애가 뭔 타입인지 지 알아서 집어오고 c#이 설정해줌. 안좋음 , 다른 개발자가 읽기 불편함 => 해결
        if (attackReult.Count <= 0)
        {
            brain.GetMove(infos.EnemyMove.VectorList, infos.EnemyAttack.VectorList); //없으면 이동
        }
        else
        {
            EnemySpcAct(); //있으면 행동실행 상속받아서 
        }
        Vector3Int v3int = grid.WorldToCell(transform.position);
        BoardManager.Instance.TileCompos[v3int].SetOccupie(gameObject);
    }
    public IEnumerator EnemyCortine()
    {
        while (CurrentEnergy > 0) //태윤이꺼는 에너지로 공격, 이동을 하지만 짜피 에너미는 에너지를 참조할 필요가 없음.
        {
            myturn = true;
            if (attack.EnemyAttackend == true&&Job == true)
            {
                EnemyNorAct();
                CurrentEnergy--;
            }
            yield return new WaitForSeconds(0.5f);
        } // 프로퍼티로 maxEnergy 만들고 저장, 이거 와일문 끝난 뒤 Energy = MaxEnergy
    }
    public abstract void EnemySpcAct();

    public void ReduceHealth(int damage)
    {
        
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        
    }

    public void Die()
    {
        
    }
}
