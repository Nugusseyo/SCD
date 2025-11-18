using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code;
using Work.PTY.Scripts;

public class StatEventManager : MonoBehaviour, IEvent
{
    private bool isPlayer = false; // 플레이어 인지, 에너미 인지 확인할때 true일때는 플레이어, false일때는 에너미

    private bool isAttack; // 왜 만들었을까 : isAttack이 true면 공격력, false면 체력
    public bool TargetBoth { get; private set; } // 플레이어랑, 에너미 둘다 선택 하고 싶을때 true로 해줌

    private string textMessage;

    private int value;

    public int[] SaveFakeHealth => MyValue(_statManager.PieceHealth);
    public int[] SaveFakeDamage=> MyValue(_statManager.PieceDamage);
    public int[] SaveRealHealth => _statManager.PieceHealth;
    public int[] SaveRealDamage => _statManager.PieceDamage;

   // public int[] ReturnDamageValue => 

    private int[] MyValue(int[] realValue)
    {
        int[] returnValue = new int[realValue.Length];
        for(int i = 0; i < realValue.Length; i++)
        {
            returnValue[i] = realValue[i] + Mathf.RoundToInt((float)realValue[i] / 100 * value);
        }
        return returnValue;
    }

    public bool IsEnd { get; set; }
    //Target만을 담는 List 만들기. (에너미와 플레이어 둘 다 GameObject니까 우선 GameObject 리스트로 만들기.
    List<GameObject> targetList = new List<GameObject>();
    EventManager _eventManager = EventManager.Instance;
    StatManager _statManager = StatManager.Instance;

    [ContextMenu("SetBool")]
    public void SetBool()
    {
        ////isPlayer, isAttack에 true or false 넣어주기 (50% 확률)
        ////value 값 넣어주기 (단위 : 5배율. [예시 : 5%, 10%, 15%, 65% 등. 13% 안됨])
        ////IEvent 인터페이스를 상속받아서 SetBool 메서드 실행해주기

        //Target List 초기화 해주기
        targetList.Clear();
        textMessage = string.Empty;

        if(!TargetBoth)
        {
            isPlayer = Random.Range(0, 2) == 1;
        }


        value = Random.Range(-11, 21) * 5;
        if(value == 0)
        {
            value = -60;
        }
        isAttack = Random.Range(0, 2) == 1;


        if (!TargetBoth)
        {
            if (isPlayer)
            {

                Debug.Log("플레이어 대상");
                //리스트 담기 플레이어만
                foreach(Piece testplayer in _eventManager.testPlayerList)
                {
                    GameObject playergameobj = testplayer.gameObject;
                    targetList.Add(playergameobj);
                }
                textMessage = "플레이어의";
            }
            else
            {
                Debug.Log("에너미 대상");
                //리스트 담기 에너미만
                foreach(TestEnemyScrip testEnemy in _eventManager.testEnemyList)
                {
                    GameObject enemygameobj = testEnemy.gameObject;
                    targetList.Add(enemygameobj);
                }
                textMessage = "적의";
            }
        }
        else
        {
            textMessage = "모두의";
            foreach (Piece testplayer in _eventManager.testPlayerList)
            {
                GameObject playergameobj = testplayer.gameObject;
                targetList.Add(playergameobj);
            }
            foreach (TestEnemyScrip testEnemy in _eventManager.testEnemyList)
            {
                GameObject enemygameobj = testEnemy.gameObject;
                targetList.Add(enemygameobj);
            }
        }

        if (isAttack)
        {
            textMessage += " 공격력을 ";
            foreach (Piece player in _eventManager.testPlayerList)
            {

            }
        }
        else
        {
            textMessage += " 체력을 ";
            Debug.Log("체력 업");
        }
        textMessage += $" {value}% ";
        if(value > 0)
        {
            textMessage += "증가시킵니다.";
        }
        else
        {
            textMessage += "감소시킵니다.";
        }
            Debug.Log(textMessage);
        //에너미가 대상이고 공격력을 value만큼 바꿔줄 때 : "에너미의 공격력을 value%만큼 (올립/내립)니다." 출력
        //플레이어가 대상이고 체력 value만큼 바꿔줄 때 : "플레이어 체력을 value%만큼 (올립/내립)니다." 출력
        //올립, 내립의 조건 : 0보다 크다, 작다
    }

    public void StartEvent()
    {
        SetBool();
    }
}
