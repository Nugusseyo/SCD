using UnityEngine;
using Work.JYG.Code.UI.UIContainer;

namespace JYG.Code.UI
{
    public class TestPlayer : MonoBehaviour, IStatusable
    {
        [field:SerializeField] public string Name { get; set; } = "Pawn";
        [field:SerializeField] public int Power { get; set; } = 2;
        [field:SerializeField] public float Hp { get; set; } = 100;
        [field:SerializeField] public float CurrentHealth { get; set; } = 15;
    }
}
