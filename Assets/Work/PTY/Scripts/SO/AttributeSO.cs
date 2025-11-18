using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

[CreateAssetMenu(fileName = "Attribute", menuName = "SO/Attribute")]
public class AttributeSO : ScriptableObject
{
    public string attributeName;
    public int dmgChangeAmt;
    public int hpChangeAmt;
    public ObjectVectorListSO vectorList;
    public Sprite attributeImage;
}
