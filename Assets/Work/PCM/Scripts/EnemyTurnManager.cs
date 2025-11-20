
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
    [SerializeField] private List<Vector3Int> list = new List<Vector3Int>();
    [SerializeField] private GameObject EnemySprite;
    [SerializeField] private GameObject BossSprite;
    public ObjectVectorListSO EnemylistSO;
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
            EnemySpawn();
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
            for (int i = 0; i < Gameobjectlist.Count; i++)
            {
                if (Gameobjectlist[i].GetComponent<Enemy>().enabled == false)
                {
                    Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
                } //딴데로 빼던가 해야할듯
            }
        }

    }

    public void EnemySpawn()
    {

        Debug.Log("끝남");
        int rand = Random.Range(0, list.Count);
        Vector3Int spawn = list[rand];
        Debug.Log(spawn);

        // 선택한 위치 제거
        list.RemoveAt(rand);


        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        Enemy listenemy = a.GetComponent<Enemy>();
        SpriteRenderer em = a.GetComponentInChildren<SpriteRenderer>();
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

    public void Jobend()
    {
        for (int i = 0; i < Gameobjectlist.Count; i++)
        {
            Gameobjectlist[i].Jobend = false;
        }
    }
}