using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public List<Vector3> EnemyMoveList =  new List<Vector3>();
    public List<Vector3> EnemyAttackList = new List<Vector3>();
    public int Hp;
    public int Attack;
}
