using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public abstract class Enemy : MonoBehaviour
{
    public EnemyMoveSO move;
    public AgentStatSO stat;
    public EnemyBrain brain;
    public EnemyAttack attack;
    public List<int> playerHps { get; set; }
    [field:SerializeField]public int Hp { get; set; }
    [field:SerializeField]public int Attack { get; set; }
   
    private void Awake()
    {
        Hp = stat.hp;
        Attack = stat.attack;  
        brain = GetComponent<EnemyBrain>();
        attack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            EnemyNorAct();
        }
    }

    public void EnemyNorAct()
    {
            var attackReult = attack.AttackCheck(move.EnemyAttack.VectorList); //공격가능한 애 감지
            if (attackReult.Count <= 0)
            {
                   
                brain.GetMove(move.EnemyMove.VectorList, move.EnemyAttack.VectorList); //없으면 이동
            }
            else
            {
                EnemySpcAct(); //있으면 행동실행 상속받아서
            }
            this.playerHps = playerHps;
    }

    public abstract void EnemySpcAct();
}
