using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Work.PTY.Scripts.PieceManager;
using YGPacks;
using Random = UnityEngine.Random;

public class EventManager : Singleton<EventManager> //추가적으로 Monobehaviour의 성질을 가진다. // Singeton 안에 Monobehaviour가 들어있다.
{

    [SerializeField] public Button turnButton;

    public List<Piece> testPlayerList = new List<Piece>();
    public List<Enemy> testEnemyList = new List<Enemy>();
    List<IEvent> eventList = new List<IEvent>();

    public int GameTurn { get; private set; }

    public bool IsEventActivate { get; private set; }
    public Action OnTurnChanged;

    // protected가 뭐임 : 부모, 자식간의 참조를 허용해주는거
    // override가 뭐임 : 덮여쓰기, 부모, 자식 출력하고 싶을때 부모 출력하고 다시 덮여쓰고 자식꺼 출력
    // virtual은 ? : override하고 싶은 얘들 핑을 찍어 놓는다.없으면 override불가능
    // base.Awake를 왜 해줌 : 부모를 먼저 awake하고 자식을 awake하기 위해
    public void AddList(Piece player) //이거 참고해서 매개변수로 IEvent를 받아온 다음, EventList에 받아온걸 넣어주는 코드 작성
    {
        testPlayerList.Add(player);
    }

    public void AddList(Enemy enemy)
    {
        testEnemyList.Add(enemy);
    }

    public void AddList(IEvent eventManager)
    {
        eventList.Add(eventManager);
    }

    public void RemoveList(Enemy removeEnemy)
    {
        testEnemyList.Remove(removeEnemy);
    }
    public void RemoveList(Piece removePlayer)
    {
        testPlayerList.Remove(removePlayer);
    }

    public void OnTurnButtonClick()
    {
        turnButton.enabled = false; // 버튼의 Interactable을 꺼줘도 된다. 지금 방식이 문제가 있으면 Interactable을 꺼주는 방식으로 바꿀거임.
        //Enable이 뭐하는건데 꺼줘? 비활성화 시켜주는건데 여기서 예로 들자면, 버튼 눌렀을때 아무것도 할수 없는 상태로 만들어주는거
        //껐을때 버튼이 어떻게 돼? 클릭은 불가능 == 버튼의 기능을 꺼둔다.
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        PieceManager.Instance.OnAttack?.Invoke();//너가 구현할 코드가 아니다.

        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
        //플레이어를 담는 리스트를 만든다.
        //플레이어가 담긴 리스트를 foreach를 통해서 Attack을 해준다. 
        //플레이어가 IsEnd 상태가 될때까지 멈춘다.
        //끝났다면, EnemyTurn을 실행해준다.
    }
    private IEnumerator EnemyTurn()
    {
        foreach (Enemy enemy in testEnemyList)
        {
            enemy.EnemyNorAct();
            yield return new WaitUntil(() => enemy.IsEnd);
            enemy.IsEnd = false;
        }
        EnemyTurnManager.Instance.EnemySpawn();
        yield return new WaitForSeconds(2f);
        StartCoroutine(EventTrun());

        //에너미가 담는 리스트를 만든다.
        //에너미가 담긴 리스트를 foreach를 통해서 Attack을 해준다.
        //에너미가 IsEnd 상태가 될때까지 멈춘다.
        //끝났다면, EnemyTurn을 실행해준다.
    }

    private IEnumerator EventTrun()
    {
        // ?? = r.r(~);
        //랜덤으로 출력을 해줘야하니까, Random.Range로 List 인덱스 중 하나를 랜덤으로 들고 온다.
        //들고 온 IEvent를 실행해준다.
        //IEvent속 IsEnd가 True가 될때까지 잠시 코루틴을 멈추어준다.
        //이벤트가 끝난다.
        int i = Random.Range(0, eventList.Count); // ?? 머하는거임 :0부터 eventlistcount까지 랜덤한 정수를 가져온다
        //eventList.Count가 뭐하는건디 // List에 있는 개수를 새서 반환을 한다.
        if (eventList[i] == null) //이건 왜 해줘? 안하면 머가 되는ㄴ딩? // 이벤트리스트[i] 가 아무것도 없을때 끝내주기위해서
        {
            TurnButtonEnd();
            yield return null; //끝내줘요 따봉
        }
        else
        {
            eventList[i].StartEvent(); // StartEvent가 어디에 있는건데 뭘 알고 해줌?
                                       // -> eventlist[i] -> IEvent를 가져온다라는것
            yield return new WaitUntil(() => eventList[i].IsEnd); //~~까지 기다린다 -> ~~까지 = 안에 있는 메서드

            eventList[i].IsEnd = false;
            TurnButtonEnd();
        }
    }
    private void TurnButtonEnd()
    {
        turnButton.enabled = true;
        GameTurn++;
        OnTurnChanged?.Invoke();
    }
}
