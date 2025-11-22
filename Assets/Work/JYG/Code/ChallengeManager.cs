using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class ChallengeManager : Singleton<ChallengeManager>
    {
        private RectTransform _myObj;
        [SerializeField] private RectTransform height;

        protected override void Awake()
        {
            base.Awake();
            _myObj = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            ResetMyPos();
        }

        [ContextMenu("ResetPos")]
        public void ResetMyPos()
        {
            _myObj.localPosition = new Vector2(0, height.sizeDelta.y);
        }
    }
}
