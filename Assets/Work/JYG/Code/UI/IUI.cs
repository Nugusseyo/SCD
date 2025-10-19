using UnityEngine;
using UnityEngine.Events;

namespace Work.JYG.Code.UI
{
    public interface IUI
    {
        string Name { get; }
        GameObject GameObject { get; }
        void OpenSelf();
        void CloseSelf();
    }
}