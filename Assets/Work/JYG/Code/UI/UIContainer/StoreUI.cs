using UnityEngine;
using Work.JYG.Code.UI.UIContainer;

public class StoreUI : UIBase
{
    public override string Name => "Store";
    public override GameObject GameObject => gameObject;
    public override void OpenSelf()
    {
        Debug.Log("상점 Open");
    }

    public override void CloseSelf()
    {
        Debug.Log("상점 Close");
    }
}
