using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code;
using YGPacks;
using Random = UnityEngine.Random;

public class EventManager : Singleton<EventManager>
{
    private List<Enemy> enemyList = new List<Enemy>();
    
    private List<IEvent> eventList = new List<IEvent>();

    private bool _isPlayerTrun = true; //초기값은 나중에 다시 설정 ( JSON 안에서 다시 불러오기 )

    [ContextMenu("이벤트 시작")]
    public void StartEvent() // StartEvent에서 이벤트를 스타트한다 X
                             // // StartEvent에서 eventList 안에 0부터 ~ event 안에 있는 요소를 찾고, 그 속에 IEvent의 StartEvent를 해준다
    {
        eventList[Random.Range(0, eventList.Count)].StartEvent();
    }
    
    public void AddList(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void AddList(IEvent iEvent)
    {
        Debug.Log("UI 추가됨 ");
        eventList.Add(iEvent);
    }
    
    public void RemoveList(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
    public void RemoveList(IEvent iEvent)
    {
        eventList.Remove(iEvent);
    }
    // enemyList 생성
    //AddList() => enemy Class -> List
    //RemoveList => enemy Class -> List.EnemyAdress.리스트에서 삭제

    public void SwapTurn() //버튼을 눌렀을 때 //SwapTrun을 해줄 때, 플레이어 턴이면 Input 켜주고 아니면 꺼준다 X
    {
        _isPlayerTrun = !_isPlayerTrun; // 플레이어 턴 -> 에너미, 에너미 -> 플레이어 턴
        
        if (_isPlayerTrun)
        {
            InputManager.Instance.PlayerInput.TestInput.Enable();
            //뭔갈 실행 가능!
            //Input을 So로 처리해줄거임. 
        }
        else
        {
            InputManager.Instance.PlayerInput.TestInput.Disable();
            //에너미 List에 있는 모든 적의 알고리즘을 실행
            StartCoroutine(PlayEnemy());

            //지금 에너미 턴인데?
            //상호작용 있는게 X
            // disable
            //버튼 활성화
        }
    }

    private IEnumerator PlayEnemy() //얘는 설명하지 말기
    {
        // List, Array => 정렬이 가능한 // 값 반환이 1번하고 끝나지 않아
        //List [1] -> [2] -> [3] -> [4] 
        foreach (Enemy enemy in enemyList)
        {
            //첫번째로 실행했을 때 : enemy[]
            //enemy.알고리즘시작 (철민이가 만들어줄거임)
            //yield return new WaitUntil(enemy.already);
            //enemy.already = false;
        }

        yield return new WaitForSeconds(1f);
    }
}
