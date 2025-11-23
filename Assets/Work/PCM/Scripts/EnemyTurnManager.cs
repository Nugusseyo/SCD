
using DG.Tweening;
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
    [SerializeField] private List<Vector3Int> list = new List<Vector3Int>();
    [SerializeField] private GameObject EnemySprite;
    [SerializeField] public GameObject BossSprite;
    public List<GameObject> Bosstlist = new List<GameObject>();
    public ObjectVectorListSO EnemylistSO;
    public int turn; //나중에 도현이가 만든 턴 메니저로 바꾸기
    private int rand;

    public Action BossAction;
    protected override void Awake()
    {
        base.Awake();
        if (grid == null)
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
        if (list.Count == 0)
        {

            for (int i = 0; i < EnemylistSO.VectorList.Count; i++)
            {
                list.Add(EnemylistSO.VectorList[i]);
            }
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("ada");
            StartCoroutine(EnemyTurn());
        }
    }
    public IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        foreach (Enemy enemy in EventManager.Instance.testEnemyList)
        {
            enemy.EnemyRealSpawn();
            yield return new WaitUntil(() => enemy.IsEnd);
            yield return new WaitForSeconds(0.3f);
            enemy.IsEnd = false;
        }
        if (EventManager.Instance.GameTurn != 0 && EventManager.Instance.GameTurn % 20 == 0)
        {
            BossEnemySpawn();
        }
        else
        {
            SpawnCondiction();
        }
        yield return new WaitForSeconds(2f);
        EventManager.Instance.StartCoroutine(EventManager.Instance.EventTrun());
    }
    public void SpawnCondiction()
    {
        if (EventManager.Instance.GameTurn >= 20)
        {
            for (int i = 0; i < Random.Range(EventManager.Instance.GameTurn / 20, (EventManager.Instance.GameTurn / 20) + 1)+1; i++)
                EnemySpawn();
        }
        else
        {
            EnemySpawn();
        }
    }
    public void BossEnemySpawn()
    {
        rand = Random.Range(0, list.Count);
        Vector3Int spawn = list[rand];
        list.RemoveAt(rand);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        GameObject a = Instantiate(Bossenemy[(EventManager.Instance.GameTurn / 20) - 1]);
        a.transform.position = enemytrs;
        Enemy listenemy = a.GetComponent<Enemy>();
        SpriteRenderer em = a.GetComponentInChildren<SpriteRenderer>();
        em.sprite = BossSprite.GetComponent<SpriteRenderer>().sprite;
        Bosstlist.Add(listenemy.gameObject);
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
        EventManager.Instance.AddList(listenemy);
        listenemy.enabled = false;

    }
}