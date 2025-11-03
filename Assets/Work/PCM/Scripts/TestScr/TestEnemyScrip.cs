using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TestEnemyScrip : MonoBehaviour, ITurnAble
{
    public Action OnEnemyAttack;
    public Action OnEnemyMove;

    //IEnemyAttackable
    //EnemyAttack

    public EnemyMoveSO move; // 둘이 병합해서 EnemySO로 결합하기 // 에너미 성격도 SO 안에 결합하기
    public AgentStatSO stat;
    protected EnemyBrain brain;// 얘네 둘도 프로퍼티로 만들어줘도 됨
    protected EnemyAttack attack; // 얘네 둘도 프로퍼티로 만들어줘도 됨 싫음 말고
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Attack { get; set; }
    public int Energy { get; set; } = 8;
    public bool IsEnd { get; set; } = false; // 이후에 Json으로 저장

    private void Awake()
    {
        Hp = stat.hp;
        Attack = stat.attack;
        brain = GetComponent<EnemyBrain>();
        attack = GetComponentInChildren<EnemyAttack>(); //EnemyBrain, EnemyAttack은 객체로 만들어서 에너미 안에 GameObject로 만들기
        // GetComponetnInChilderen으로 들고오기 , 싫음 말고

        OnEnemyAttack += HandleEnemyAttackEvent;
    }
    private void OnDestroy()
    {
        OnEnemyAttack -= HandleEnemyAttackEvent;
    }

    private void HandleEnemyAttackEvent()
    {
        attack.AOE(stat.attack);
        //소리
        //이펙트 등
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            StartCoroutine(EnemyCortine());
        }
    }
    public void EnemyNorAct()
    {
        List<Vector3Int> attackReult = attack.AttackCheck(move.EnemyAttack.VectorList); //공격가능한 애 감지
        //var = 애가 뭔 타입인지 지 알아서 집어오고 c#이 설정해줌. 안좋음 , 다른 개발자가 읽기 불편함 => 해결 
        if (attackReult.Count <= 0)
        {
            brain.GetMove(move.EnemyMove.VectorList, move.EnemyAttack.VectorList); //없으면 이동
        }
        else
        {
            EnemySpcAct(); //있으면 행동실행 상속받아서 
        }
    }
    private IEnumerator EnemyCortine()
    {
        while (Energy > 0) //태윤이꺼는 에너지로 공격, 이동을 하지만 짜피 에너미는 에너지를 참조할 필요가 없음.
        {
            Energy--;
            EnemyNorAct();
            yield return new WaitForSeconds(0.5f);
        } // 프로퍼티로 maxEnergy 만들고 저장, 이거 와일문 끝난 뒤 Energy = MaxEnergy
    }
    public abstract void EnemySpcAct();
}
