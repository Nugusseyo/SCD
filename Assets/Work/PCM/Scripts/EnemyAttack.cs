using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using Work.PTY.Scripts;
using static UnityEditor.PlayerSettings;

public class EnemyAttack : MonoBehaviour, IDamageable
{
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask unit;
    private readonly List<Vector3Int> playerList = new();
    private readonly List<Piece> hits = new();
    [field: SerializeField] public bool EnemyAttackend { get; set; } = false;
    public int AttackDamage { get; set; }

    public void Awake()
    {
        EnemyAttackend = true;
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
            Collider2D hit = Physics2D.OverlapPoint(worldPos, unit);
            if (hit)
            {
                hits.Add(hit.gameObject.GetComponent<Piece>());
                playerList.Add(cellPos);
            }
        }
        return playerList;
    }
    public void AOE(int damage)
    {
        for (int i = 0; i < hits.Count; i++)
        {
            EnemyAttackend = false;
            var starpos = transform.position;
            int index = i;
            if (!gameObject.CompareTag("Boss"))
            {
                transform.DOMoveY(hits[i].transform.position.y + 0.2f, 0.5f).SetEase(Ease.InBack, 3f)
                .OnComplete(() =>
                {
                    hits[index].GetComponent<IDamageable>().TakeDamage(damage, gameObject);
                    Debug.Log($"{starpos}");
                    transform.DOMove(starpos, 0.2f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        EnemyAttackend = true;
                    });
                });
            }
            
        }
    }
    public void FastEnemyAttack(int damage)
    {
        Debug.Log("험");
        for (int i = 0; i < hits.Count; i++)
        {
            int index = i;
            Debug.Log(hits[index]);
            hits[index].GetComponent<IDamageable>().TakeDamage(damage, gameObject);                           
        }
    }
    public void RangedAttack(Piece player, int damage)
    {
        Debug.Log("일단됨");
        player.GetComponent<IDamageable>().TakeDamage(damage,gameObject);
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
         
    }

    public void Die()
    {
        //잘모르겠음
    }
}
