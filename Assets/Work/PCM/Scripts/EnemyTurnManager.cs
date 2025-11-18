
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.JYG.Code.Chessboard.Pieces;
using Random = UnityEngine.Random;


public class EnemyTurnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] enemy;
    [SerializeField]private Grid grid;
    [SerializeField]private List<GameObject> Gameobjectlist = new List<GameObject>();
    public ObjectVectorListSO EnemylistSO;
    public void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
    }
    public void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)//나중에 턴 으로 변경
        {
            StartCoroutine(EnemySequence());
            
        }
    }

    private void EnemySpawn()
    {
        #region 원래 코드
        //for (int i = 0; i < Gameobjectlist.Count; i++)
        //{
        //    if (Gameobjectlist[i].GetComponent<TestEnemyScrip>().enabled == false)
        //    {
        //        Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
        //    }
        //    else if (Gameobjectlist[i].GetComponent<TestEnemyScrip>().enabled == true)
        //    {
        //        var Enemy = Gameobjectlist[i].GetComponent<TestEnemyScrip>();
        //        Enemy.Job = true;
        //        if (i > 0)
        //        {
        //            if (Gameobjectlist[i - 1].GetComponent<TestEnemyScrip>().Job == false)
        //            {
        //                Debug.Log($"{Gameobjectlist[i]}시작");
        //                Enemy.StartCoroutine(Enemy.EnemyCortine());
        //            }
        //            else
        //            {
        //                Debug.Log($"{Gameobjectlist[i]}내 앞{Gameobjectlist[i-1]}");
        //            }
        //        }
        //        else if (i == 0)
        //        {
        //            Enemy.StartCoroutine(Enemy.EnemyCortine());
        //        }
        //    }
        //}
        #endregion
        //Debug.Log("끝남");
        //int rand = Random.Range(0, EnemylistSO.VectorList.Count);
        //Vector3Int spawn = EnemylistSO.VectorList[rand];

        //GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        //var enemytrs = grid.GetCellCenterWorld(spawn);
        //a.transform.position = enemytrs;
        //Gameobjectlist.Add(a);
        //a.GetComponent<TestEnemyScrip>().enabled = false;
    }
    IEnumerator EnemySequence()
    {
        for (int i = 0; i < Gameobjectlist.Count; i++)
        {
            var enemy = Gameobjectlist[i].GetComponent<TestEnemyScrip>();

            // Enemy 비활성 → 스폰 시간
            if (!enemy.enabled)
            {
                Gameobjectlist[i].GetComponent<EnemySpawn>().SpawnTime();
                continue;
            }

            // Enemy 활성 → 일 시작
            enemy.Job = true;

            // i > 0 → 앞 Enemy가 끝날 때까지 기다림
            if (i > 0)
            {
                var previous = Gameobjectlist[i - 1].GetComponent<TestEnemyScrip>();

                // 앞 enemy가 Job == true (작업 중) 이면 기다려야 함
                yield return new WaitUntil(() => previous.Job == false);
            }

            // 이제 이 enemy 시작
            yield return enemy.StartCoroutine(enemy.EnemyCortine());
        }
        int rand = Random.Range(0, EnemylistSO.VectorList.Count);
        Vector3Int spawn = EnemylistSO.VectorList[rand];

        GameObject a = Instantiate(enemy[Random.Range(0, enemy.Length)]);
        var enemytrs = grid.GetCellCenterWorld(spawn);
        a.transform.position = enemytrs;
        Gameobjectlist.Add(a);
        a.GetComponent<TestEnemyScrip>().enabled = false;
        Debug.Log("전체 끝남");
    }


}