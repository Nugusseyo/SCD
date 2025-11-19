using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

[CreateAssetMenu(fileName = "PieceList", menuName = "SO/PieceList")]
public class PieceListSO : ScriptableObject
{
    public PieceSO[] pieces;
    public ObjectVectorListSO[] vectorLists;
}
