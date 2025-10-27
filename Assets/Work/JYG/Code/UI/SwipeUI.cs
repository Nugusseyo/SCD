using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Work.JYG.Code.UI
{
    public class SwipeUI : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollBar;
        private int _currentPage = 0;
        private int _maxPage = 0;
        private Image[] _childImg;
        
        private float _imgWidthBase;
        private float[] _imgWidth;

        private Vector2 _startPos;
        private Vector2 _endPos;

        private float _minLength = 100f;
        private float _swipeDuration = 0.1f;

        public bool IsSwipeMode { get; private set; }
        
        private void Awake()
        {
            _childImg = new Image[transform.childCount];
            _imgWidth = new float[transform.childCount];
            _maxPage = transform.childCount;
            _imgWidthBase = 1f / (_imgWidth.Length - 1f); //이미지 한 장당 거리를 1을 쪼개어 슬라이드 기준, 몇인지를 알아냄.
        }

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _childImg[i] = transform.GetChild(i).GetComponent<Image>();
                _imgWidth[i] = i * _imgWidthBase;
            }
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _startPos = Input.touches[0].position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _endPos = Input.touches[0].position;

                ScrollMove();
            }
            
        }

        private void ScrollMove()
        {
            if (IsSwipeMode) return;
            
            if (Mathf.Abs(_startPos.x - _endPos.x) > _minLength)
            {
                bool isLeft =  _startPos.x < _endPos.x;
                if (isLeft)
                {
                    if (_currentPage == 0)
                        return;
                    _currentPage--;
                }
                else
                {
                    if (_currentPage == _maxPage - 1)
                        return;
                    _currentPage++;
                }
                StartCoroutine(OnSwipeUI(_currentPage));
            }
            else
            {
                StartCoroutine(OnSwipeUI(_currentPage));
            }
        }

        private IEnumerator OnSwipeUI(int currentPage)
        {
            Debug.Log("StartCoroutine");
            float start = scrollBar.value;
            float current = 0;
            float percent = 0;

            IsSwipeMode = true;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / _swipeDuration;
                
                scrollBar.value = Mathf.Lerp(start, _imgWidth[currentPage], percent);
                
                yield return null;
                
            }

            IsSwipeMode = false;
        }
    }
}
