using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using YGPacks.PoolManager;

public class EnemyTester : MonoBehaviour, IPoolable
{
    public string Name => "Enemy";
    List<Enemy> list = new List<Enemy>();

    //int i = 5;
    //list ;

    public GameObject GameObject => gameObject;

    public bool IsReady { get; private set; } = false; //IsReady가 플레이어 턴이 아닐때
    // 시작할때 나는 EventManager 안 리스트에 들어갈거에요

    private void Start()
    {
        
    } 
    public void EnemyAttack()
    {
        IsReady = false; //아닐때 공격시작
        Debug.Log("공격해쪄염승민");
        StartCoroutine(Attacking());
        
    }

    private IEnumerator Attacking() //한개 헀다가 꺼냈다가 뭔가를 계속 반복해서 할때 IEnumerator를 사용한다.
    {
        yield return new WaitForSeconds(1f); // return을 사용하면 안된는 이유은ㄴ return은 한번하면 끝나는데 yield리턴은 계속 이라는 의미? 그런 기능을 해서 yield를 붙여준다.
        IsReady = true;//다시 턴 바꿔줌
    }
    [ContextMenu("주거")] //주거를 했을때
    public void Dead()
    {
      //  TesterEvManager.Instance.RemoveList(this);
        PoolManager.Instance.Push(this); //Push가 저장
    }

    public void AppearanceItem()
    {
        transform.position = Vector3.zero;
      //  TesterEvManager.Instance.AddList(this);
    }

    public void ResetItem()
    {
        
    }
}
