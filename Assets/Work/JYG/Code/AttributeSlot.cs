using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code;

public class AttributeSlot : MonoBehaviour
{
    public AttributeSO myAttribute;

    [SerializeField] private Image myImage;
    [SerializeField] private Button myButton;
    [SerializeField] private TextMeshProUGUI myName;
    [SerializeField] private TextMeshProUGUI myDescription;

    private void Awake()
    {
        myImage.sprite = myAttribute.attributeImage;
        myName.text = myAttribute.attributeName;
        myDescription.text = myAttribute.attributeDescription;
    }

    public void SetMyAttribute()
    {
        Debug.Log("Set Attribute");
        if (AttributeUiManager.Instance.CurrentPiece != null)
        {
            AttributeUiManager.Instance.CurrentPiece.Attributes.Add(myAttribute);
            AttributeUiManager.Instance.CurrentPiece.OnAttributeChanged?.Invoke();
            foreach (Button btn in AttributeUiManager.Instance.attributeBtnList)
            {
                btn.interactable = false;
            }
            StartCoroutine(DelayUpdate(AttributeUiManager.Instance.CurrentPiece));
            AttributeUiManager.Instance.UiClose();
            TileChecker.Instance.RemoveMySelCompo();
            //TileChecker.Instance.SelPcCompo.attributes.
        }
    }

    private IEnumerator DelayUpdate(Piece piece)
    {
        yield return null;
        piece.UpdateUI();
    }
}
