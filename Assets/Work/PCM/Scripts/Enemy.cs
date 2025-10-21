using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO stat;
    [SerializeField] private LayerMask unit;
    [SerializeField] Grid grid;
    private Grid ChessGrid;
    private int _hp;
    private int _attack;
    Dictionary<Vector3Int, int> moveValue = new Dictionary<Vector3Int, int>();
    private Ray ray;
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
                //Debug.Log(stat.EnemyMoveList[i]); 잘됨 
                Vector3Int current = grid.WorldToCell(transform.position);
                
                Vector3Int exceptionCell =  grid.WorldToCell(current+ stat.EnemyMoveList[i]);
                Brain(exceptionCell);
                moveValue.Add(stat.EnemyMoveList[i],count);        
            }
            var trans = moveValue.OrderByDescending(x => x.Value).First().Key;
            transform.position += trans;
            
        }
    }
    private void Brain(Vector3Int movePos)
    {
        count = 0;
        for (int i = 0; i < stat.EnemyAttackList.Count; i++)
        {
            Vector3Int attackOffset = stat.EnemyAttackList[i];
            Vector3Int centerCell = movePos + attackOffset;

            // 셀 좌표 → 월드 좌표로 변환 (필요 시)
            Vector3 centerWorld = grid.CellToWorld(centerCell);

            // OverlapBox는 Vector2를 사용하므로 변환
            Vector2 center = (Vector2)centerWorld;
            Vector2 boxSize = Vector2.one;

            Collider2D hit = Physics2D.OverlapBox(center, boxSize, 0f, unit);
            if (hit)
                count++;
        }
    }
}
