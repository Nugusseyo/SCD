using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public ObjectVectorListSO EnemyMove;
    public ObjectVectorListSO EnemyAttack;
    public int Hp;
    public int Attack;
}
