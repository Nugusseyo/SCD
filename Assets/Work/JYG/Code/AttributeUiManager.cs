using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class AttributeUiManager : Singleton<AttributeUiManager>
    {
        [SerializeField] private GameObject attributeUi;
        public List<Button> attributeBtnList =  new List<Button>(); 
        public Piece CurrentPiece { get; private set; }
        private List<bool> _attributeCanActivate = new List<bool>();
        private List<GameObject> _attributeGameObj = new List<GameObject>();

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < 10; i++)
            {
                _attributeCanActivate.Add(false);
            }
        }

        private void Start()
        {
            attributeBtnList = GetComponentsInChildren<Button>().ToList();
            _attributeGameObj = attributeBtnList.Select(x => x.gameObject).ToList();
            attributeUi.SetActive(false);
            CanAttributeOn();
        }

        public void CanAttributeOn()
        {
            try
            {
                if (_attributeCanActivate.Count == 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        _attributeCanActivate[i] = (PlayerPrefs.GetInt("C" + i) == 1);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        _attributeGameObj[i].SetActive(_attributeCanActivate[i]);
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log("몬다이나이");
            }
        }

        public void UiOpen(Piece currentPiece)
        {
            EventManager.Instance.TurnMyInput(false);
            CurrentPiece = currentPiece;
            attributeUi.SetActive(true);
            foreach (Button btn in AttributeUiManager.Instance.attributeBtnList)
            {
                btn.interactable = true;
                if (btn.gameObject.TryGetComponent<AttributeSlot>(out AttributeSlot attributeSlot))
                {
                    foreach (AttributeSO attribute in CurrentPiece.Attributes)
                    {
                        if (attributeSlot.myAttribute == attribute)
                        {
                            btn.interactable = false;
                        }
                    }
                }
            }
        }

        public void UiClose()
        {
            CurrentPiece = null;
            attributeUi.SetActive(false);
            StartCoroutine(TurnInputTrue());
        }

        private IEnumerator TurnInputTrue()
        {
            yield return null;
            EventManager.Instance.TurnMyInput(true);
        }
    }
}
