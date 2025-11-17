using UnityEngine;

namespace Work.JYG.Code
{
    public class TestPice : MonoBehaviour, ITestPiecable
    {
        public int Health { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
    }
}
