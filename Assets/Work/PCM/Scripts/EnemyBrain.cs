using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public enum Type
{
    warlike,warunlike
}
public class EnemyBrain : MonoBehaviour
{

    [Header("성격 정하기")]
    [field:SerializeField]private Type type;


    readonly Dictionary<Vector3Int, int> moveValue = new();
    [Header("레이어")]
    [SerializeField] private LayerMask player;
    [SerializeField] private LayerMask unit;

    [Header("그리드")]
    [SerializeField]private Grid grid;
    private int count = 0;
    private Vector3Int trans;
    private void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
    }
    public void GetMove(List<Vector3Int> MoveAble ,List<Vector3Int> Attacks ) //stat.EnemyMoveList[i]
    {
        moveValue.Clear();
        for (int i = 0; i < MoveAble.Count; i++)
        {
            Vector3Int current = grid.WorldToCell(transform.position);
            Vector3Int exceptionCell = current + MoveAble[i];
            
            GetAttack(Attacks, exceptionCell);
            moveValue.Add(exceptionCell, count);
            if (exceptionCell.x > 7 || exceptionCell.x < 0 || exceptionCell.y > 7 || exceptionCell.y < 0)
            {
                moveValue.Remove(exceptionCell);
            }
            Vector3 worldPos = grid.GetCellCenterWorld(exceptionCell);
            Collider2D enemys = Physics2D.OverlapPoint(worldPos, unit); //앞으로 이동할수 있나 확인
            if (enemys)
            {
                moveValue.Remove(exceptionCell);
            }
        }
        if (moveValue.Count == 0)
            trans = grid.WorldToCell(transform.position);
        else if (type == Type.warlike)
        {
            trans = moveValue.OrderByDescending(x => x.Value).First().Key;
        }
        else if (type == Type.warunlike)
            trans = moveValue.OrderBy(x => x.Value).First().Key;
        

            Vector3 enemyMove = grid.GetCellCenterWorld(trans);
        transform.DOMove(enemyMove,0.2f);
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
            Vector2 boxSize = new Vector2(0.9f,1);

            Collider2D hit = Physics2D.OverlapBox(center, boxSize, 0f, player);
            if (hit)
            {
                Debug.Log(grid.WorldToCell(hit.gameObject.transform.position));

                count++;
            }
        }
    }
}
