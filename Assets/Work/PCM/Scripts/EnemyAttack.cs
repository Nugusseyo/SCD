using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using Work.PTY.Scripts;
using static UnityEditor.PlayerSettings;

public class EnemyAttack : MonoBehaviour ,IDamageable
{
    [SerializeField]private Grid grid;
    [SerializeField]private LayerMask unit;
    private readonly List<Vector3Int> playerList = new();
    private readonly List<TestPlayerStat> hits = new();

    public void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
    }
    public List<Vector3Int> AttackCheck(List<Vector3Int> Attack) //hp받아올려고 튜플로 만듬
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
            TakeDamage(damage, hits[i].gameObject);
        }
    }
    public void RangedAttack(TestPlayerStat player,int damage)
    {
        TakeDamage(damage,player.gameObject);
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        attacker.GetComponent<IAgentHealth>().CurrentHealth -= damage;
    }

    public void Die()
    {
        //잘모르겠음
    }
}
