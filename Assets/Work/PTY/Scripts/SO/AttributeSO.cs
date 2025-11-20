using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

[CreateAssetMenu(fileName = "Attribute", menuName = "SO/Attribute")]
public class AttributeSO : ScriptableObject
{
    public string attributeName;
    public int dmgUpPercent;
    public int hpUpPercent;
    public int energyUpAmount;
    public int enemyDmgReducePercent;
    public bool canHeal; //25% heals other pieces when it's on attacking piece's vector list
    public ObjectVectorListSO additionalVectorList;
    public Sprite attributeImage;
}
