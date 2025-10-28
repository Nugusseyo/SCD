using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]private Grid grid;
    [SerializeField]private LayerMask unit;
    private List<Vector3Int> playerList = new List<Vector3Int>();
    private List<TestPlayerStat> hits = new List<TestPlayerStat>();

    private void Awake()
    {
       playerList = new List<Vector3Int>();
    }
    public List<Vector3Int> AttackCheck(List<Vector3Int> Attack) //hp¹Þ¾Æ¿Ã·Á°í Æ©ÇÃ·Î ¸¸µë
    {
        playerList.Clear();
        hits.Clear();
        for (int i = 0; i < Attack.Count; i++)
        {
            Vector3Int center = grid.WorldToCell(transform.position);
            Vector3Int cellPos = center + Attack[i];
            Vector3 worldPos = grid.GetCellCenterWorld(cellPos); 
            Collider2D hit = Physics2D.OverlapPoint(worldPos,unit);
            if (hit)
            {
                hits.Add(hit.gameObject.GetComponent<TestPlayerStat>());
                playerList.Add(cellPos);
            }            
        }
        return playerList;
    }
    public void Attack(int damage)
    {
        for (int i = 0; i < hits.Count; i++)
        {
            hits[i].Hp -= damage;
        }
    }
}
