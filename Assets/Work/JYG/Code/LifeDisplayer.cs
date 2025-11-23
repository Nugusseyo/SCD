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
            OffMyUI();
            ReloadLife();
        }

        public void ReloadLife()
        {
            text.text = PlayerPrefs.GetInt("Life", 3).ToString();
            if (PlayerPrefs.GetInt("Life") <= 0)
            {
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

        public void OffMyUI()
        {
            EventManager.Instance.TurnMyInput(true);
            EventManager.Instance.TurnMyGraphicRaycast(true);
            image.localPosition += new Vector3(0, image.sizeDelta.y, 0);
        }

    }
}
