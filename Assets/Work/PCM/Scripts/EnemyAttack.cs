using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]private Grid grid;
    [SerializeField]private LayerMask unit;
    public bool GetAttack(List<Vector3Int> Attack)
    {
        for (int i = 0; i < Attack.Count; i++)
        {
            Vector3Int center = grid.WorldToCell(transform.position);
            Vector3 range = center + Attack[i];
            Debug.Log(range);
            Vector2 size = Vector2.one;
            Collider2D hit = Physics2D.OverlapBox((Vector2)range, size, 0, unit);
            if (hit)
                return true;
            
        }
        return false;
    }
}
