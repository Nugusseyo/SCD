using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class ChallengeManager : Singleton<ChallengeManager>
    {
        private RectTransform _myObj;
        [SerializeField] private RectTransform height;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI details;

        [SerializeField] private Image icon;

        [SerializeField] private Sprite[] icons;

        public Action OnChallengeSwitchContacted;

        protected override void Awake()
        {
            base.Awake();
            _myObj = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            ResetMyPos();
            OnChallengeSwitchContacted += HandleChallengeDetector;
        }

        private void HandleChallengeDetector()
        {
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnChallengeSwitchContacted -= HandleChallengeDetector;
        }


        [ContextMenu("ResetPos")]
        public void ResetMyPos()
        {
            _myObj.localPosition = new Vector2(0, height.sizeDelta.y);
        }

        [ContextMenu("ShowTitle")]
        public void DownMyPos(string title, string details, int iconIndex)
        {
            this.title.text = title;
            this.details.text = details;
            this.icon.sprite = icons[iconIndex];
            _myObj.DOAnchorPos(new Vector2(0, 0), 0.8f).SetEase(Ease.InOutQuart);
            StartCoroutine(DisShowTitle());
        }

        private IEnumerator DisShowTitle()
        {
            yield return new WaitForSeconds(5f);
            UpMyPos();
        }

        [ContextMenu("DisableTitle")]
        public void UpMyPos()
        {
            _myObj.DOAnchorPos(new Vector2(0, height.sizeDelta.y), 0.8f).SetEase(Ease.InQuart);
        }
    }
}
