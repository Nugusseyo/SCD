using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentHealthSystem", menuName = "Scriptable Objects/AgentHealthSystem")]
public class AgentStatSO : ScriptableObject
{
    public int hp;
    public int attack;
    public int coin;

    private int hiddenHp;
    private int hiddenAttack;
    private int hiddenCoin;
    public void StartStat()
    {
        hiddenHp = hp;
        hiddenAttack = attack;
        hiddenCoin = coin;
        Debug.Log("da");
    }
    public void UpStat()
    {

        hp += (int)(hiddenHp * 40 / 20 - 0.8);
        attack += (int)(hiddenAttack * EnemyTurnManager.Instance.turn / 20 - 0.8);
        coin += (int)(hiddenCoin * EnemyTurnManager.Instance.turn/ 20 - 0.8);

        Debug.Log("�ϴ� ����");

    }
    public void ReStart()
    {
        attack = hiddenAttack;
        hp = hiddenHp;
        coin = hiddenCoin;
    }
}
