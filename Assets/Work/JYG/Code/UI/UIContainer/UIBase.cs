using System;
using UnityEngine;

namespace Work.JYG.Code.UI.UIContainer
{
    public abstract class UIBase : MonoBehaviour, IUI
    {
        public int Index { get; protected set; }
        public abstract string Name { get; }
        public abstract GameObject GameObject { get; }
        public abstract void OpenSelf();
        public abstract void CloseSelf();

        protected virtual void Start()
        {
            UIManager.Instance.AddUI(this);
        }
    }
}
