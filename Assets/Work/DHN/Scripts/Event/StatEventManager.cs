using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Work.JYG.Code;
using Work.PTY.Scripts;

public class StatEventManager : MonoBehaviour, IEvent
{
    private bool isPlayer = false; // �÷��̾� ����, ���ʹ� ���� Ȯ���Ҷ� true�϶��� �÷��̾�, false�϶��� ���ʹ�

    private bool isAttack; // �� ��������� : isAttack�� true�� ���ݷ�, false�� ü��
    public bool TargetBoth { get; private set; } // �÷��̾��, ���ʹ� �Ѵ� ���� �ϰ� ������ true�� ����

    private string textMessage;

    private int value;

    private const int EVENT_TURN = 2;
    private int offTurn;

    public int[] SaveFakeHealth => MyValue(StatManager.Instance.PieceHealth); 
    public int[] SaveFakeDamage => MyValue(StatManager.Instance.PieceDamage);
    public int[] SaveRealHealth => StatManager.Instance.PieceHealth;
    public int[] SaveRealDamage => StatManager.Instance.PieceDamage;

    public int[] ReturnHealth { get; private set; }
    public int[] ReturnDamage { get; private set; }

    private void OnEnable()
    {
        EventManager.Instance.OnTurnChanged += HandleTurnDetect;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTurnChanged -= HandleTurnDetect;
    }

    private void HandleTurnDetect()
    {
        if(offTurn == EventManager.Instance.GameTurn)
        {
            ValueGoBack();
        }
        else if(offTurn > EventManager.Instance.GameTurn)
        {
            ValueChangeFake();
        }

        Debug.Log("Turn Change Detected");
    }

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
    //Target���� ��� List �����. (���ʹ̿� �÷��̾� �� �� GameObject�ϱ� �켱 GameObject ����Ʈ�� �����.
    List<GameObject> targetList = new List<GameObject>();

    [ContextMenu("SetBool")]
    public void SetBool()
    {
        ////isPlayer, isAttack�� true or false �־��ֱ� (50% Ȯ��)
        ////value �� �־��ֱ� (���� : 5����. [���� : 5%, 10%, 15%, 65% ��. 13% �ȵ�])
        ////IEvent �������̽��� ��ӹ޾Ƽ� SetBool �޼��� �������ֱ�

        //Target List �ʱ�ȭ ���ֱ�
        targetList.Clear();
        textMessage = string.Empty;
        offTurn = EventManager.Instance.GameTurn + EVENT_TURN;

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

                Debug.Log("�÷��̾� ���");
                //����Ʈ ��� �÷��̾
                foreach(Piece testplayer in EventManager.Instance.testPlayerList)
                {
                    GameObject playergameobj = testplayer.gameObject;
                    targetList.Add(playergameobj);
                }
                textMessage = "�÷��̾���";
            }
            else
            {
                Debug.Log("���ʹ� ���");
                //����Ʈ ��� ���ʹ̸�
                foreach(TestEnemyScrip testEnemy in EventManager.Instance.testEnemyList)
                {
                    GameObject enemygameobj = testEnemy.gameObject;
                    targetList.Add(enemygameobj);
                }
                textMessage = "����";
            }
        }
        else
        {
            textMessage = "�����";
            foreach (Piece testplayer in EventManager.Instance.testPlayerList)
            {
                GameObject playergameobj = testplayer.gameObject;
                targetList.Add(playergameobj);
            }
            foreach (TestEnemyScrip testEnemy in EventManager.Instance.testEnemyList)
            {
                GameObject enemygameobj = testEnemy.gameObject;
                targetList.Add(enemygameobj);
            }
        }

        if (isAttack)
        {
            textMessage += " ���ݷ��� ";
            foreach (Piece player in EventManager.Instance.testPlayerList)
            {

            }
        }
        else
        {
            textMessage += " ü���� ";
            Debug.Log("ü�� ��");
        }
        textMessage += $" {value}% ";
        if(value > 0)
        {
            textMessage += "������ŵ�ϴ�.";
        }
        else
        {
            textMessage += "���ҽ�ŵ�ϴ�.";
        }
            Debug.Log(textMessage);
        //���ʹ̰� ����̰� ���ݷ��� value��ŭ �ٲ��� �� : "���ʹ��� ���ݷ��� value%��ŭ (�ø�/����)�ϴ�." ���
        //�÷��̾ ����̰� ü�� value��ŭ �ٲ��� �� : "�÷��̾� ü���� value%��ŭ (�ø�/����)�ϴ�." ���
        //�ø�, ������ ���� : 0���� ũ��, �۴�
    }

    public void StartEvent()
    {
        SetBool();
    }

    private void ValueGoBack()
    {
        foreach (Piece player in EventManager.Instance.testPlayerList)
        {
            /*
            bool afterPlusHealth = false;
            if (SaveFakeHealth[player.pieceData.pieceIndex] < SaveRealHealth[player.pieceData.pieceIndex])
            {// 가짜 Health가 진짜 Health보다 더 작을 때, 이후에 체력 증가해주기
                afterPlusHealth = true;
            }
            if (player.CurrentHealth > SaveRealHealth[player.pieceData])
            {// 플레이어의 현재 체력이 저장된 진짜 체력보다 클 때, 현재 체력을 진짜 체력으로 감소시켜주기
                player.CurrentHealth = SaveRealHealth[pieceIndex];
            }

            if (afterPlusHealth)
            {
                player.TakeDamage(SaveFakeHealth[player.pieceData.pieceIndex] - SaveRealHealth[player.pieceData.pieceIndex]);
            }
            */
        }
        
        StatManager.Instance.ReturnPieceDamage = SaveRealDamage;
        StatManager.Instance.ReturnPieceHealth = SaveRealHealth;
    }

    private void ValueChangeFake()
    {
        StatManager.Instance.ReturnPieceDamage = SaveFakeDamage;
        StatManager.Instance.ReturnPieceHealth = SaveFakeHealth;
    }

}
