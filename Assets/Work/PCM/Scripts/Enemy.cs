using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO stat;
    private EnemyBrain brain;
    private EnemyAttack attack;
    private int _hp;
    private int _attack;
   
    private void Awake()
    {
        _hp = stat.Hp;
        _attack = stat.Attack;  
        brain = GetComponent<EnemyBrain>();
        attack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        EnemyNorAct();
    }

    private void EnemyNorAct()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            var attackReult = attack.GetAttack(stat.EnemyAttack.VectorList); //공격가능한 애 감지
            if (attackReult.Count <= 0)
            {
                brain.GetMove(stat.EnemyMove.VectorList, stat.EnemyAttack.VectorList); //없으면 이동
            }
            else
            {
                EnemySpcAct(); //있으면 행동실행 상속받아서
            }    
        }
    }

    public virtual void EnemySpcAct()
    {

    }
}
