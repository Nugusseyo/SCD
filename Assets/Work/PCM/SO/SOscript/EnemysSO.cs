using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO")]
public class EnemysSO : ScriptableObject
{
    public ObjectVectorListSO EnemyMove;
    public ObjectVectorListSO EnemyAttack;
    public AgentStatSO EnemyStat;
    public int Energy;
    public int Spawning;
    public ObjectVectorListSO EnemyAttack2;
}
