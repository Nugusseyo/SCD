using UnityEngine;

public enum PieceType
{
    King, Queen, Bishop, Knight, Rook, Pawn
}

[CreateAssetMenu(fileName = "Piece", menuName = "SO/Piece")]
public class PieceSO : ScriptableObject
{
    public PieceType type;
    public int attributeAmount;
    public Sprite sprite;
    public Vector2[] interactableTiles;
}
