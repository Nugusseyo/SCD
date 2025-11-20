using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSlot : MonoBehaviour
{
    [SerializeField] private AttributeSO myAttribute;

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
        if (TileChecker.Instance.SelPcCompo != null)
        {
            Debug.Log(myAttribute.attributeName + "Detected, Set My Attribute in " + TileChecker.Instance.SelPcCompo.Name);
            //TileChecker.Instance.SelPcCompo.attributes.
        }
    }
}
