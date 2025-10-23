using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBrain : MonoBehaviour
{

    [SerializeField] private LayerMask player;
    [SerializeField] private LayerMask unit;
    Dictionary<Vector3Int, int> moveValue = new Dictionary<Vector3Int, int>();
    [SerializeField]private Grid grid;
    private int count = 0;
    public void GetMove(List<Vector3Int> MoveAble ,List<Vector3Int> Attacks ) //stat.EnemyMoveList[i]
    {
        moveValue.Clear();
        for (int i = 0; i < MoveAble.Count; i++)
        {
            Vector3Int current = grid.WorldToCell(transform.position);
            Vector3Int exceptionCell = current + MoveAble[i];
            Vector3 worldPos = grid.GetCellCenterWorld(exceptionCell);

            GetAttack(Attacks, exceptionCell);
            moveValue.Add(exceptionCell, count);

            Collider2D enemys = Physics2D.OverlapPoint(worldPos, unit);
            if (enemys)
            {
                Debug.Log("Àû °¨ÁöµÊ: " + enemys.name);
                moveValue.Remove(exceptionCell);
            }
        }

        var trans = moveValue.OrderByDescending(x => x.Value).First().Key;
        transform.position = grid.GetCellCenterWorld(trans);
    }
    public void GetAttack(List<Vector3Int> AttackAble, Vector3Int movePos)
    {
        count = 0;
        for (int i = 0; i < AttackAble.Count; i++)
        {
            Vector3Int attackOffset = AttackAble[i];
            Vector3Int centerCell = movePos + attackOffset;
            Vector3 centerWorld = grid.CellToWorld(centerCell);
            Vector2 center = (Vector2)centerWorld;
            Vector2 boxSize = Vector2.one;

            Collider2D hit = Physics2D.OverlapBox(center, boxSize, 0f, player);
            if (hit)
                count++;
        }
    }
}
