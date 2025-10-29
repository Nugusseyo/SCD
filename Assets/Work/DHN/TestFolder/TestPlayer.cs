using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour, ITestTurnInterface
{
    public bool IsEnd { get; set; }
    public int Energy { get; set; }

    public void Activity()
    {
        Debug.Log("플레이어가 공격하겠읍니다.");
        StartCoroutine(EnemyDoAttack());
    }

    private IEnumerator EnemyDoAttack()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("공격이 모두 끝났읍니다.");
        IsEnd = true;
    }

    private void Start()
    {
        EventManager.Instance.AddList(this);
    }
}
