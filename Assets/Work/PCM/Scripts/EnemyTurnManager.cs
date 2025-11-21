
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
    [SerializeField] private GameObject[] Bossenemy;
    [SerializeField] private Grid grid;
    [SerializeField] private List<Enemy> Gameobjectlist = new List<Enemy>();
    [SerializeField] private List<Vector3Int> list = new List<Vector3Int>();
    [SerializeField] private List<Vector3Int> Bosslist = new List<Vector3Int>();
    [SerializeField] private GameObject EnemySprite;
    [SerializeField] private GameObject BossSprite;
    public ObjectVectorListSO EnemylistSO;
    public int turn; //나중에 도현이가 만든 턴 메니저로 바꾸기
    private int rand;
    public void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
    }
    private void Start()
    {
        for (int i = 0; i < EnemylistSO.VectorList.Count; i++)
        {
            list.Add(EnemylistSO.VectorList[i]);
        }
    }
    public void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)//나중에 턴 으로 변경
        {
            if (turn !=0&&turn % 20 == 0)
            {
                BossEnemySpawn();
            }
            else
            {
                EnemySpawn();
            }
            turn++;
            Jobend();
        }
        if (list.Count== 0)
        {
            
            for (int i = 0; i < EnemylistSO.VectorList.Count; i++)
            {
                list.Add(EnemylistSO.VectorList[i]);
            }
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            EnemyRealSpawn();           
        }
    }

    private void EnemyRealSpawn()
    {
        for (int i = 0; i < Gameobjectlist.Count; i++)
        {
            if (Gameobjectlist[i].GetComponent<Enemy>().enabled == false)
            {
                Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
            } 
        }
    }

    public void BossEnemySpawn()
    {
        rand = Random.Range(0, list.Count);
        Vector3Int spawn = list[rand];
        list.RemoveAt(rand);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        GameObject a = Instantiate(Bossenemy[Random.Range(0, Bossenemy.Length)]);
        a.transform.position = enemytrs;
        Enemy listenemy = a.GetComponent<Enemy>();
        SpriteRenderer em = a.GetComponentInChildren<SpriteRenderer>();
        em.sprite = BossSprite.GetComponent<SpriteRenderer>().sprite;
        Gameobjectlist.Add(listenemy);
        EventManager.Instance.AddList(listenemy);
        listenemy.enabled = false;
    }
    public void EnemySpawn()
    {
        rand = Random.Range(0, list.Count);
        Vector3Int spawn = list[rand];
        list.RemoveAt(rand);
        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        Enemy listenemy = a.GetComponent<Enemy>();
        SpriteRenderer em = a.GetComponentInChildren<SpriteRenderer>();
        em.sprite = EnemySprite.GetComponent<SpriteRenderer>().sprite;
        em.color = Color.black;

        listenemy.IsEnd = true;
        Gameobjectlist.Add(listenemy);
        EventManager.Instance.AddList(listenemy);
        listenemy.enabled = false;
    }

    public void Jobend()
    {
        for (int i = 0; i < Gameobjectlist.Count; i++)
        {
            Gameobjectlist[i].IsEnd = false;
        }
    }

    
}