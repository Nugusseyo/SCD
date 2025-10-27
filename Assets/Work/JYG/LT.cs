using UnityEngine;

public class LT : Event
{
    public override void StartEvent() //event를 받아오고 있고 event들어가 보면 메서드 start메서드를 호출하고 있기때문에 이벤트 시작시 콘솔창에 바로 띄어줌 
    {
        Debug.Log("번개가 떨어진당!!");
    }
}
