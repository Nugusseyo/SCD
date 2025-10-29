using UnityEngine;

public interface IEvent
{
    void StartEvent();
    bool IsEnd { get; set; }
}
