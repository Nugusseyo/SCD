using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO stat;
    private int _hp;
    private int _attack;
    Dictionary<Vector2, int> moveValue = new Dictionary<Vector2, int>();
    private Ray ray;
    [SerializeField] private LayerMask unit;
    private int count = 0;

   
    private void Awake()
    {
        _hp = stat.Hp;
        _attack = stat.Attack;  
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            moveValue.Clear();
            for (int i =0 ; i < stat.EnemyMoveList.Count; i++)
            {
                //Debug.Log(stat.EnemyMoveList[i]); ภ฿ตส 
                Brain(transform.position+stat.EnemyMoveList[i]);
                moveValue.Add(stat.EnemyMoveList[i],count);        
            }
            transform.position +=(Vector3)moveValue.OrderByDescending(x => x.Value).First().Key;
            
        }
    }
    private void Brain(Vector3 movePos)
    {
        count = 0;
        for (int i = 0; i < stat.EnemyAttackList.Count; i++)
        {
            Vector2 attackOffset = stat.EnemyAttackList[i];
            Vector2 center = (Vector2)movePos + attackOffset;
            Debug.Log($"center:{center} movePos:{movePos} attackOFfset:{attackOffset}");

            Vector2 boxSize = Vector2.one;

            Collider2D hit = Physics2D.OverlapBox(center, boxSize, 0f, unit);
            if (hit)
                count++;
        }
    }
}
