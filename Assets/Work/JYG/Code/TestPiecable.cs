using UnityEngine;

namespace Work.JYG.Code
{
    public interface ITestPiecable
    {
        public int Health { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
    }
}