using UnityEngine;

public enum PieceType
{
    King, Queen, Bishop, Knight, Rook, Pawn
}

[CreateAssetMenu(fileName = "Piece", menuName = "SO/Piece")]
public class PieceSO : ScriptableObject
{
    public int pieceIndex;
    public PieceType type;
    public int attributeAmount;
    public Sprite sprite;

    public int health;
    public int damage;
    public int healthIncAmt;
    public int damageIncAmt;
}
