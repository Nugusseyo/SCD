using System;
using UnityEngine;
using Work.JYG.Code.UI;
using Work.JYG.Code.UI.UIContainer;
using TouchPhase = UnityEngine.TouchPhase;

public class InputManagerUI : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    
    private UIManager uiManager;

    private void Start()
    {
        
        uiManager = UIManager.Instance;
    }

    private void Update()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began && Input.touchCount > 0)
        {
            startPosition = Input.GetTouch(0).position;
        }

        if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.touchCount > 0)
        {
            //동그라미 화살표가 손가락 따라가기
            if (IsDrag())
            {
                //uiManager.
            }
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended && Input.touchCount > 0)
        {
            endPosition = Input.GetTouch(0).position;
            if (IsDrag())
            {
                Debug.Log("Swap Detected");
                if (startPosition.x > endPosition.x)
                {
                    Swap2Right();
                }
            }
        }
    }

    private void Swap2Right()
    {
        
    }

    private void Swap2Left()
    {
        
    }

    private bool IsDrag()
    {
        return Mathf.Abs(startPosition.x - endPosition.x) > 0.4f;
    }
}
