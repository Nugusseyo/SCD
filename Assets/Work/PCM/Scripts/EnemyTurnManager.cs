
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
    [SerializeField] private List<Enemy> Gameobjectlist = new List<Enemy>();
    [SerializeField]private List<Vector3> list = new List<Vector3>();
    [SerializeField]private GameObject EnemySprite;
    [SerializeField] private GameObject BossSprite;
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
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            for (int i = 0; i < Gameobjectlist.Count; i++)
            {
                if (Gameobjectlist[i].GetComponent<Enemy>().enabled == false)
                {
                    Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
                } //딴데로 빼던가 해야할듯
                #region
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
                #endregion
            }
        }

    }

    public void EnemySpawn()
    {
        
        Debug.Log("끝남");
        int rand = Random.Range(0, EnemylistSO.VectorList.Count);
        Vector3Int spawn = EnemylistSO.VectorList[rand];
        list.Remove(spawn);

        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        Enemy listenemy = a.GetComponent<Enemy>();
        SpriteRenderer em= a.GetComponentInChildren<SpriteRenderer>();
        if (a.CompareTag("Boss"))
        {
            em.sprite = BossSprite.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            em.sprite = EnemySprite.GetComponent<SpriteRenderer>().sprite;
            em.color = Color.black;
        }

        listenemy.Jobend = true;
        Gameobjectlist.Add(listenemy);
        EventManager.Instance.AddList(listenemy);
        listenemy.enabled = false;
    } 

    //public void EnemySpawns()
    //{
    //    int rand = Random.Range(0, EnemylistSO.VectorList.Count);
    //    Vector3Int spawn = EnemylistSO.VectorList[rand];
    //    EnemylistSO.VectorList.RemoveAt(rand);

    //    GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
    //    var enemytrs = grid.GetCellCenterWorld(spawn);
    //    a.transform.position = enemytrs;
    //    TestEnemyScrip listenemy = a.GetComponent<TestEnemyScrip>();
    //    Gameobjectlist.Add(listenemy);
    //    listenemy.Jobend = false;
    //    listenemy.enabled = false;
        
    //}
    //private void EnemyMove()
    //{
    //    if (Gameobjectlist.Count > 0)
    //    {
    //        var enemy = Gameobjectlist[0].GetComponent<TestEnemyScrip>();
    //        if (!enemy.enabled)
    //        {
    //            Gameobjectlist[0].GetComponent<EnemySpawn>().SpawnTime();
    //        }
    //        else if (enemy.enabled)
    //        {
               
    //        }
    //    }
    //    EnemySpawns();
        
    //}
    public void Jobend()
    {
        for(int i = 0;i<Gameobjectlist.Count; i++)
        {
            Gameobjectlist[i].Jobend = false;
        }
    }
}