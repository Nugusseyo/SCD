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

    public EnemysSO infos; // ���� �����ؼ� EnemySO�� �����ϱ� // ���ʹ� ���ݵ� SO �ȿ� �����ϱ�
    protected EnemyBrain brain;// ��� �ѵ� ������Ƽ�� ������൵ ��
    protected EnemyAttack attack; // ��� �ѵ� ������Ƽ�� ������൵ �� ���� ����
    [field: SerializeField] public bool Jobend { get; set; } = false;

    public bool IsEnd { get; set; } = false; // ���Ŀ� Json���� ����
    public int MaxEnergy { get; set; }
    [field: SerializeField] public int CurrentEnergy { get; set; }
    [SerializeField]private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } set {currentHealth= Mathf.Clamp(value,0,MaxHealth); }}
    [field:SerializeField]public int MaxHealth { get; set; }
    public bool IsDead { get ; set; }
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
        attack = GetComponentInChildren<EnemyAttack>(); //EnemyBrain, EnemyAttack�� ��ü�� ���� ���ʹ� �ȿ� GameObject�� �����
        // GetComponetnInChilderen���� ������� , ���� ����
        mySprite = GetComponentInChildren<SpriteRenderer>();
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
        //����Ʈ ��
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            StartCoroutine(EnemyCortine());
        }
        if (CurrentEnergy <= 0&&attack.EnemyAttackend == true&&myturn == true)
        {
            StopAllCoroutines();    
            Debug.Log($"{gameObject},�� ��");
            myturn = false;
            Jobend = true;
            IsEnd = true;
            CurrentEnergy = MaxEnergy;
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
            EnemySpcAct(); //������ �ൿ���� ��ӹ޾Ƽ� 
        }
        Vector3Int v3int = grid.WorldToCell(transform.position);
        BoardManager.Instance.TileCompos[v3int].SetOccupie(gameObject);
    }
    public IEnumerator EnemyCortine()
    {
        while (CurrentEnergy > 0) 
        {
            Debug.Log("�߸�");
            myturn = true;
            if (attack.EnemyAttackend == true&&Jobend == false)
            {
                EnemyNorAct();
                CurrentEnergy--;
            }
            yield return new WaitForSeconds(0.5f);
        }
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
        EventManager.Instance.RemoveList(this);   
        Destroy(gameObject);
    }
}
