using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]private Grid grid;
    [SerializeField]private LayerMask unit;
    private List<Vector3Int> enemyList;

    private void Awake()
    {
       enemyList = new List<Vector3Int>();
    }
    public List<Vector3Int> GetAttack(List<Vector3Int> Attack)
    {
        enemyList.Clear();
        for (int i = 0; i < Attack.Count; i++)
        {
            Vector3Int center = grid.WorldToCell(transform.position);
            Vector3Int cellPos = center + Attack[i];
            Vector3 worldPos = grid.GetCellCenterWorld(cellPos); 
            Collider2D hit = Physics2D.OverlapPoint(worldPos,unit);
            if (hit)
            {
                enemyList.Add(cellPos);
            }            
        }
        return enemyList;
    }
}
