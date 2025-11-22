using System;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code;

public class AttributeAddButton : MonoBehaviour
{
   [SerializeField] private Button myButton;

   private void Start()
   {
      myButton.onClick.AddListener(OpenAttribute);
   }

   private void OpenAttribute()
   {
      if (TileChecker.Instance.SelPcCompo != null)
      {
         AttributeUiManager.Instance.UiOpen(TileChecker.Instance.SelPcCompo);
      }
   }
}
