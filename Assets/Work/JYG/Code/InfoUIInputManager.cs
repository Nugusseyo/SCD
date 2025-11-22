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
        [SerializeField] private GameObject[] infoUI;
        
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
            foreach (GameObject obj in infoUI)
            {
                obj.SetActive(false);
            }
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
                foreach (GameObject obj in infoUI)
                {
                    obj.SetActive(false);
                }
            }
        }

        private void LoadPieceInfo(Piece component)
        {
            Debug.Log("Info's Num is "+uiInfos[component.pieceData.pieceIndex - 1].index);
            nameTxt.text = uiInfos[component.pieceData.pieceIndex - 1].infoName;
            hpTxt.text = $"Health : {component.CurrentHealth}/{component.GetFinalMaxHealth()}";
            powerTxt.text = $"Attack : {component.GetFinalDamage()}";
            pieceImg.sprite = uiInfos[component.pieceData.pieceIndex - 1].icon;
            pieceImg.SetNativeSize();
            slotUI.SetActive(false);
            foreach (GameObject obj in infoUI)
            {
                obj.SetActive(true);
            }

            
            foreach (Image image in attributeImgs)
            {
                image.sprite = null;
                image.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
            }

            foreach (GameObject addBtn in attributeAddBtns)
            {
                addBtn.SetActive(false);
            }

            activeAttribute = 0;
            for (int i = 0; i < component.Attributes.Count; i++)
            {
                activeAttribute++;
                attributeImgs[i].sprite = component.Attributes[i].attributeImage;
                attributeImgs[i].gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
            }

            for (int i = 0; i < component.pieceData.attributeAmount - activeAttribute; i++)
            {
                attributeAddBtns[i].SetActive(true);
            }
        }
    }
}
