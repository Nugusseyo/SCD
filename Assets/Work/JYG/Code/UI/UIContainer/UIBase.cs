using UnityEngine;

namespace Work.JYG.Code.UI.UIContainer
{
    public abstract class UIBase : MonoBehaviour, IUI
    {
        public abstract string Name { get; }
        public abstract GameObject GameObject { get; }
        public abstract void OpenSelf();
        public abstract void CloseSelf();
    }
}
