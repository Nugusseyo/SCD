using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public List<Vector3Int> EnemyMoveList = new List<Vector3Int>();
    public List<Vector3Int> EnemyAttackList = new List<Vector3Int>();
    public int Hp;
    public int Attack;
}
