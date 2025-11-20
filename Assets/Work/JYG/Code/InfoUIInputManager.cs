using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code.UI.UIContainer;

namespace Work.JYG.Code
{
    public class InfoUIInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject infoUI;
        
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private TextMeshProUGUI hpTxt;
        [SerializeField] private TextMeshProUGUI powerTxt;
        [SerializeField] private Image pieceImg;

        [SerializeField] private GameObject slotUI;
        [SerializeField] private GameObject slot;
        [SerializeField] private List<PieceUiInfo> uiInfos = new  List<PieceUiInfo>();
        [SerializeField] private List<Image> attributeImgs = new  List<Image>();
        [SerializeField] private List<GameObject> attributeAddBtns = new  List<GameObject>();
        
        private int activeAttribute = 0;

        private void Start()
        {
            infoUI.SetActive(false);
        }

        private void Update()
        {
            if (TileChecker.Instance.SelPcCompo != null)
            {
                if (slot.activeSelf) return;
                LoadPieceInfo(TileChecker.Instance.SelPcCompo);
            }
            else
            {
                slotUI.SetActive(true);
                infoUI.SetActive(false);
            }
        }

        private void LoadPieceInfo(Piece component)
        {
            nameTxt.text = uiInfos[component.pieceData.pieceIndex].infoName;
            hpTxt.text = $"Health : {component.CurrentHealth}/{component.MaxHealth}";
            powerTxt.text = $"Attack : {component.AttackDamage}";
            pieceImg.sprite = uiInfos[component.pieceData.pieceIndex].icon;
            pieceImg.SetNativeSize();
            slotUI.SetActive(false);
            infoUI.SetActive(true);

            
            foreach (Image image in attributeImgs)
            {
                image.sprite = null;
                image.gameObject.transform.parent.gameObject.SetActive(false);
            }

            foreach (GameObject addBtn in attributeAddBtns)
            {
                addBtn.SetActive(false);
            }

            activeAttribute = 0;
            for (int i = 0; i < component.attributes.Length; i++)
            {
                activeAttribute++;
                attributeImgs[i].sprite = component.attributes[i].attributeImage;
                attributeImgs[i].gameObject.transform.parent.gameObject.SetActive(true);
            }

            for (int i = 0; i < component.pieceData.attributeAmount - activeAttribute; i++)
            {
                attributeAddBtns[i].SetActive(true);
            }
        }
    }
}
