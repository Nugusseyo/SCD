using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
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
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (attack.GetAttack(stat.EnemyAttackList))
            {
                Debug.Log("けいしけ");
            }
            brain.GetMove(stat.EnemyMoveList, stat.EnemyAttackList);
        }
    }
   
}
