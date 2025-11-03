using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]private Grid grid;
    [SerializeField]private LayerMask unit;
    private readonly List<Vector3Int> playerList = new();
    private readonly List<TestPlayerStat> hits = new();

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
    public void AOE(int damage)
    {
        for (int i = 0; i < hits.Count; i++)
        {
            hits[i].Hp -= damage;
        }
    }
    public void RangedAttack(TestPlayerStat player,int damage)
    {
        player.Hp -= damage;
    }
}
