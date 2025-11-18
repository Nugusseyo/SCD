
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.JYG.Code.Chessboard.Pieces;
using YGPacks;
using Random = UnityEngine.Random;


public class EnemyTurnManager : Singleton<EnemyTurnManager>
{
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private Grid grid;
    [SerializeField] private List<TestEnemyScrip> Gameobjectlist = new List<TestEnemyScrip>();
    [SerializeField]private List<Vector3> list = new List<Vector3>();
    public ObjectVectorListSO EnemylistSO;
    public void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
    }
    private void Start()
    {
        for(int i = 0; i < EnemylistSO.VectorList.Count; i++)
        {
            list.Add(EnemylistSO.VectorList[i]);
        }
    }
    public void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)//나중에 턴 으로 변경
        {
            EnemySpawn();
            Jobend();
        }
        if(list.Count == 0)
        {
            for (int i = 0; i < EnemylistSO.VectorList.Count; i++)
            {
                list.Add(EnemylistSO.VectorList[i]);
            }
        }
    }

    private void EnemySpawn()
    {
        for (int i = 0; i < Gameobjectlist.Count; i++)
        {
            if (Gameobjectlist[i].GetComponent<TestEnemyScrip>().enabled == false)
            {
                Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
            }
            //else if (Gameobjectlist[i].GetComponent<TestEnemyScrip>().enabled == true)
            //{
            //    var Enemy = Gameobjectlist[i].GetComponent<TestEnemyScrip>();
            //    if (i > 0)
            //    {
            //        if (Gameobjectlist[i - 1].GetComponent<TestEnemyScrip>().Jobend == false)
            //        {
            //            Debug.Log($"{Gameobjectlist[i]}시작");
            //            Enemy.StartCoroutine(Enemy.EnemyCortine());
            //        }
            //        else
            //        {
            //            Debug.Log($"{Gameobjectlist[i]}내 앞{Gameobjectlist[i - 1]}");
            //        }
            //    }
            //    else if (i == 0)
            //    {
            //        Enemy.StartCoroutine(Enemy.EnemyCortine());
            //    }
            //}
        }
        Debug.Log("끝남");
        int rand = Random.Range(0, EnemylistSO.VectorList.Count);
        Vector3Int spawn = EnemylistSO.VectorList[rand];
        list.Remove(spawn);

        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        TestEnemyScrip listenemy = a.GetComponent<TestEnemyScrip>();
        Gameobjectlist.Add(listenemy);
        //EventManager.Instance.AddList(listenemy);
        listenemy.enabled = false;
    } 
    //안씀
    //IEnumerator EnemySequence()
    //{
    //    int rand = Random.Range(0, EnemylistSO.VectorList.Count);
    //    Vector3Int spawn = EnemylistSO.VectorList[rand];

    //    GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
    //    var enemytrs = grid.GetCellCenterWorld(spawn);
    //    a.transform.position = enemytrs;
    //    Gameobjectlist.Add(a);
    //    a.GetComponent<TestEnemyScrip>().enabled = false;
    //    for (int i = 0; i < Gameobjectlist.Count; i++)
    //    {
    //        var enemy = Gameobjectlist[i].GetComponent<TestEnemyScrip>();
    //        Debug.Log(Gameobjectlist[i]);

    //        Enemy 비활성 → 스폰 시간
    //         Enemy 활성 → 일 시작
    //        enemy.Job = true;
    //        i > 0 → 앞 Enemy가 끝날 때까지 기다림
    //        if (!enemy.enabled)
    //        {
    //            Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
    //            continue;
    //        }
    //        if (i > 0)
    //        {
    //            var previous = Gameobjectlist[i - 1].GetComponent<TestEnemyScrip>();

    //            앞 enemy가 Job == true(작업 중) 이면 기다려야 함
    //           yield return new WaitUntil(() => previous.Job == false);
    //        }


    //        이제 이 enemy 시작
    //        yield return enemy.StartCoroutine(enemy.EnemyCortine());
    //    }

    //    Debug.Log("전체 끝남");
    //}  //안씀
    private void EnemySpawns()
    {
        int rand = Random.Range(0, EnemylistSO.VectorList.Count);
        Vector3Int spawn = EnemylistSO.VectorList[rand];
        EnemylistSO.VectorList.RemoveAt(rand);

        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        TestEnemyScrip listenemy = a.GetComponent<TestEnemyScrip>();
        Gameobjectlist.Add(listenemy);
        listenemy.Jobend = false;
        listenemy.enabled = false;
        
    }
    private void EnemyMove()
    {
        if (Gameobjectlist.Count > 0)
        {
            var enemy = Gameobjectlist[0].GetComponent<TestEnemyScrip>();
            if (!enemy.enabled)
            {
                Gameobjectlist[0].GetComponent<EnemySpawn>().SpawnTime();
            }
            else if (enemy.enabled)
            {
               
            }
        }
        EnemySpawns();
        
    }
    private void Jobend()
    {
        for(int i = 0;i<Gameobjectlist.Count; i++)
        {
            Gameobjectlist[i].Jobend = false;
        }
    }
}