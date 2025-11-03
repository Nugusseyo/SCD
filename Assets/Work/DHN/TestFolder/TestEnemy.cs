using System;
using System.Collections;
using UnityEngine;

public class TestEnemy : MonoBehaviour, ITestTurnInterface
{
    public bool IsEnd { get; set; }
    public int Energy { get; set; }

    public void Activity()
    {
        Debug.Log("에너미가 공격하겠읍니다.");
        StartCoroutine(EnemyDoAttack());
    }

    private IEnumerator EnemyDoAttack()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("공격이 모두 끝났읍니다.");
        IsEnd = true;
    }
}
