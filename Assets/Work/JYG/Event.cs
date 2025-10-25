using System;
using UnityEngine;
using Work.JYG.Code;

public abstract class Event : MonoBehaviour, IEvent
{
    private int value;

    protected virtual void Awake()
    {
        Initialize();
    }

    public abstract void StartEvent();
    protected virtual void Initialize()
    {
        // Add해줄거임 eventManager 안에 있는 List에 ㅇㅇ
        EventManager.Instance.AddList(this);
    }
    //추상
    
}
