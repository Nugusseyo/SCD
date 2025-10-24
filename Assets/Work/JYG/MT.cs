using UnityEngine;

public class MT : Event
{
    protected override void Awake()
    {
        base.Awake();
        //나 할거임
    }

    public override void StartEvent()
    {
        Debug.Log("메테오가 떨어진당!!");
    }
}
