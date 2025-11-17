using System;
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

        [SerializeField] private Image[] talentImg;

        [SerializeField] private GameObject slotUI;
        [SerializeField] private GameObject slot;

        private bool _canOpen = true;

        private void Start()
        {
            infoUI.SetActive(false);
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (slot.activeSelf) return;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), new Vector2(0, 0));
                if (hit.collider != null && _canOpen)
                {
                    if (hit.collider.gameObject.TryGetComponent<IStatusable>(out IStatusable component))
                    {
                        global::JYG.Code.UI.TestPlayer tp = component as global::JYG.Code.UI.TestPlayer;
                        SpriteRenderer spr = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                        if (tp == null) return;
                        Debug.Log(component.Name);
                        nameTxt.text = component.Name;
                        hpTxt.text = $"Health : {tp.CurrentHealth}/{component.Hp}";
                        pieceImg.sprite = spr.sprite;
                        _canOpen = false;
                        slotUI.SetActive(false);
                        infoUI.SetActive(true);
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
    }
}
