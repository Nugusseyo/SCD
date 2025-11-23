using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class LifeDisplayer : Singleton<LifeDisplayer>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private RectTransform image;
        private void Start()
        {
            image.localPosition += new Vector3(0, image.sizeDelta.y, 0);
        }

        public void ReloadLife()
        {
            text.text = PlayerPrefs.GetInt("Life", 3).ToString();
            if (PlayerPrefs.GetInt("Life") <= 0)
            {
                Debug.Log("폭망");
                MoveMyUI();
            }
        }
        [ContextMenu("MoveUI")]
        private void MoveMyUI()
        {
            EventManager.Instance.TurnMyInput(false);
            EventManager.Instance.TurnMyGraphicRaycast(false);
            image.DOAnchorPosY(0, 1f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                image.GetComponent<GraphicRaycaster>().enabled = true;
            });
            

        }

    }
}
