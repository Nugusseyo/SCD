using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Work.PTY.Scripts.PieceManager;
using YGPacks;
using Random = UnityEngine.Random;

public class EventManager : Singleton<EventManager> //�߰������� Monobehaviour�� ������ ������. // Singeton �ȿ� Monobehaviour�� ����ִ�.
{
    [field:SerializeField] public PlayerInputSO UserInput { get; private set; }
    [SerializeField] public Button turnButton;

    public List<Piece> testPlayerList = new List<Piece>();
    public List<Enemy> testEnemyList = new List<Enemy>();
    List<IEvent> eventList = new List<IEvent>();

    [SerializeField] private GraphicRaycaster bottomUiCanvas;

    public int GameTurn { get; private set; }

    public bool IsEventActivate { get; private set; }
    public Action OnTurnChanged;

    [SerializeField] private Button debuggingBtn;
    public bool debugIsOk = true;

    // protected�� ���� : �θ�, �ڽİ��� ������ ������ִ°�
    // override�� ���� : ��������, �θ�, �ڽ� ����ϰ� ������ �θ� ����ϰ� �ٽ� �������� �ڽĲ� ���
    // virtual�� ? : override�ϰ� ���� ��� ���� ��� ���´�.������ override�Ұ���
    // base.Awake�� �� ���� : �θ� ���� awake�ϰ� �ڽ��� awake�ϱ� ����
    public void AddList(Piece player) //�̰� �����ؼ� �Ű������� IEvent�� �޾ƿ� ����, EventList�� �޾ƿ°� �־��ִ� �ڵ� �ۼ�
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
        turnButton.interactable = false; // ��ư�� Interactable�� ���൵ �ȴ�. ���� ����� ������ ������ Interactable�� ���ִ� ������� �ٲܰ���.
        //Enable�� ���ϴ°ǵ� ����? ��Ȱ��ȭ �����ִ°ǵ� ���⼭ ���� ���ڸ�, ��ư �������� �ƹ��͵� �Ҽ� ���� ���·� ������ִ°�
        //������ ��ư�� ��� ��? Ŭ���� �Ұ��� == ��ư�� ����� ���д�.
        TurnMyInput(false);
        bottomUiCanvas.enabled = false;
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player Turn");
        PieceManager.Instance.OnAttack?.Invoke();//�ʰ� ������ �ڵ尡 �ƴϴ�.

        yield return new WaitUntil(() => PieceManager.Instance.IsAttacking == false);
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
        //�÷��̾ ��� ����Ʈ�� �����.
        //�÷��̾ ��� ����Ʈ�� foreach�� ���ؼ� Attack�� ���ش�. 
        //�÷��̾ IsEnd ���°� �ɶ����� �����.
        //�����ٸ�, EnemyTurn�� �������ش�.
    }
    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        foreach (Enemy enemy in testEnemyList)
        {
            enemy.EnemyRealSpawn();
            yield return new WaitUntil(() => enemy.IsEnd);
            yield return new WaitForSeconds(0.3f);
            enemy.IsEnd = false;
        }
        EnemyTurnManager.Instance.EnemySpawn();
        yield return new WaitForSeconds(2f);
        StartCoroutine(EventTrun());
        //���ʹ̰� ��� ����Ʈ�� �����.
        //���ʹ̰� ��� ����Ʈ�� foreach�� ���ؼ� Attack�� ���ش�.
        //���ʹ̰� IsEnd ���°� �ɶ����� �����.
        //�����ٸ�, EnemyTurn�� �������ش�.
    }

    public IEnumerator EventTrun()
    {
        Debug.Log("Event Turn");
        // ?? = r.r(~);
        //�������� ����� ������ϴϱ�, Random.Range�� List �ε��� �� �ϳ��� �������� ��� �´�.
        //��� �� IEvent�� �������ش�.
        //IEvent�� IsEnd�� True�� �ɶ����� ��� �ڷ�ƾ�� ���߾��ش�.
        //�̺�Ʈ�� ������.
        int i = Random.Range(0, eventList.Count); // ?? ���ϴ°��� :0���� eventlistcount���� ������ ������ �����´�
        //eventList.Count�� ���ϴ°ǵ� // List�� �ִ� ������ ���� ��ȯ�� �Ѵ�.
        if (eventList[i] == null) //�̰� �� ����? ���ϸ� �Ӱ� �Ǵ¤���? // �̺�Ʈ����Ʈ[i] �� �ƹ��͵� ������ �����ֱ����ؼ�
        {
            TurnButtonEnd();
            yield return null; //������� ����
        }
        else
        {
            eventList[i].StartEvent(); // StartEvent�� ��� �ִ°ǵ� �� �˰� ����?
                                       // -> eventlist[i] -> IEvent�� �����´ٶ�°�
            yield return new WaitUntil(() => eventList[i].IsEnd); //~~���� ��ٸ��� -> ~~���� = �ȿ� �ִ� �޼���

            eventList[i].IsEnd = false;
            TurnButtonEnd();
        }
    }
    private void TurnButtonEnd()
    {
        Debug.Log("End Turn");
        turnButton.interactable = true;
        GameTurn++;
        OnTurnChanged?.Invoke();
        foreach (Piece piece in testPlayerList)
        {
            piece.ResetEnergy();
            piece.UpdateUI();
        }
        bottomUiCanvas.enabled = true;
        TurnMyInput(true);
        
    }

    public void TurnMyInput(bool isTrue)
    {
        debugIsOk = isTrue;
        UserInput.TurnMyInput(isTrue);
        Debug.Log("설정을 " + isTrue);
    }
}
