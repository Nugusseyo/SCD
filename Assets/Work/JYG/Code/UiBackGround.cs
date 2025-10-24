using UnityEngine;

namespace Work.JYG.Code
{
    public class UiBackGround : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height/2);
            Debug.Log($"Camera Width, height = {Screen.width}, {Screen.height/2}");
        }
    }
}