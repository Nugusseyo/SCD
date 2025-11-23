using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Work.JYG.Code;
using Work.PTY.Scripts;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-8)]
public class StatEventManager : MonoBehaviour, IEvent
{

    private bool isAttack; // �� ��������� : isAttack�� true�� ���ݷ�, false�� ü��
    private string textMessage;

    private int value;

    private const int EVENT_TURN = 3;
    private int offTurn;

    public int[] SaveFakeHealth => MyValue(StatManager.Instance.PieceHealth); 
    public int[] SaveFakeDamage => MyValue(StatManager.Instance.PieceDamage);
    public int[] SaveRealHealth => StatManager.Instance.PieceHealth;
    public int[] SaveRealDamage => StatManager.Instance.PieceDamage;

    public int[] ReturnHealth { get; private set; }
    public int[] ReturnDamage { get; private set; }

    private void Awake()
    {
        ReturnHealth = SaveRealHealth;
        ReturnDamage = SaveRealDamage;
        StatManager.Instance.ReturnPieceDamage = ReturnDamage;
        StatManager.Instance.ReturnPieceHealth = ReturnHealth;
        HandleTurnDetect();
        
        EventManager.Instance.AddList(this);
    }

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
        foreach (Piece pieces in EventManager.Instance.testPlayerList)
        {
            pieces.UpdateUI();
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
        
        isAttack = Random.Range(0, 2) == 1;

        value = Random.Range(-11, 21) * 5;
        if (value == 0) value = -60;
        
        switch (isAttack)
        {
            case true:
                ReturnDamage = SaveFakeDamage;
                if (value > 0)
                {
                    ChallengeManager.Instance.DownMyPos("좋은 뉴스!", $"기물들의 공격력이 2턴동안 {value}% 증가합니다!", 1);
                }
                else
                {
                    ChallengeManager.Instance.DownMyPos("이런!", $"기물들의 공격력이 2턴동안 {-value}% 감소합니다!", 1);
                }
                break;
            case false:
                ReturnHealth = SaveFakeHealth;
                if (value > 0)
                {
                    ChallengeManager.Instance.DownMyPos("좋은 뉴스!", $"기물들의 체력이 2턴동안 {value}% 증가합니다!", 1);
                }
                else
                {
                    ChallengeManager.Instance.DownMyPos("이런!", $"기물들의 최대 체력이 2턴동안 {-value}% 감소합니다!", 1);
                    foreach (Piece pieces in EventManager.Instance.testPlayerList)
                    {
                        if (pieces.CurrentHealth > pieces.GetFinalMaxHealth())
                        {
                            pieces.CurrentHealth = pieces.GetFinalMaxHealth();
                        }
                    }
                }
                break;
        }
        foreach (Piece pieces in EventManager.Instance.testPlayerList)
        {
            pieces.UpdateUI();
        }
        Debug.Log(textMessage);
        IsEnd = true;
        //���ʹ̰� ����̰� ���ݷ��� value��ŭ �ٲ��� �� : "���ʹ��� ���ݷ��� value%��ŭ (�ø�/����)�ϴ�." ���
        //�÷��̾ ����̰� ü�� value��ŭ �ٲ��� �� : "�÷��̾� ü���� value%��ŭ (�ø�/����)�ϴ�." ���
        //�ø�, ������ ���� : 0���� ũ��, �۴�
    }

    public void StartEvent()
    {
        SetBool();
    }

    [ContextMenu("Go Back")]
    private void ValueGoBack()
    {
        foreach (Piece player in EventManager.Instance.testPlayerList)
        {
            try
            {
                bool afterPlusHealth = false;
                int afterValue = SaveFakeHealth[player.pieceData.pieceIndex] -
                                 SaveRealHealth[player.pieceData.pieceIndex];
                if (SaveFakeHealth[player.pieceData.pieceIndex] < SaveRealHealth[player.pieceData.pieceIndex])
                {
                    // 가짜 Health가 진짜 Health보다 더 작을 때, 이후에 체력 증가해주기
                    afterPlusHealth = true;
                }

                if (player.CurrentHealth > SaveRealHealth[player.pieceData.pieceIndex])
                {
                    // 플레이어의 현재 체력이 저장된 진짜 체력보다 클 때, 현재 체력을 진짜 체력으로 감소시켜주기
                    player.CurrentHealth = SaveRealHealth[player.pieceData.pieceIndex];
                }

                StatManager.Instance.ReturnPieceDamage = SaveRealDamage;
                StatManager.Instance.ReturnPieceHealth = SaveRealHealth;

                if (afterPlusHealth)
                {
                    player.TakeDamage(afterValue, null);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e.Message);
                Debug.Log("설마 이게 문제겠냐");
            }
            player.UpdateUI();
        }
        
        
    }
    [ContextMenu("Value Change 2 Fake")]
    private void ValueChangeFake()
    {
        StatManager.Instance.ReturnPieceDamage = SaveFakeDamage;
        StatManager.Instance.ReturnPieceHealth = SaveFakeHealth;
    }

}
