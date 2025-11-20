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

        private bool _canOpen = true;

        private void Start()
        {
            infoUI.SetActive(false);
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (slot.activeSelf) return;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), new Vector2(0, 0));
                if (hit.collider != null && _canOpen)
                {
                    if (hit.collider.gameObject.TryGetComponent<Piece>(out Piece component))
                    {
                        LoadPieceInfo(component);
                    }
                }
                else if(hit.collider == null)
                {
                    _canOpen = true;
                    slotUI.SetActive(true);
                    infoUI.SetActive(false);
                }
            }
        }

        private void LoadPieceInfo(Piece component)
        {
            Debug.Log(component.pieceData.name);
            nameTxt.text = uiInfos[component.pieceData.pieceIndex].name;
            hpTxt.text = $"Health : {component.CurrentHealth}/{component.MaxHealth}";
            powerTxt.text = $"Attack : {component.AttackDamage}";
            pieceImg.sprite = uiInfos[component.pieceData.pieceIndex].icon;
            pieceImg.SetNativeSize();
            _canOpen = false;
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
