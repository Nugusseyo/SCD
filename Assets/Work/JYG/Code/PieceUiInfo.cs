using UnityEngine;

namespace Work.JYG.Code
{
    [CreateAssetMenu(fileName = "New PieceInfo", menuName = "SO/UI/PiceInfo", order = 15)]
    public class PieceUiInfo : ScriptableObject
    {
        public Sprite icon;
        public string name;
        public int index;
    }
}