using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class SpawnsEnemy : Enemy
{
    [SerializeField]private GameObject summon;
    [SerializeField]private ObjectVectorListSO summoner;
    [SerializeField]private GameObject bullet;
    [field:SerializeField]private List<GameObject> SummonList = new List<GameObject>();
    public override void EnemySpcAct()
    {
        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.SetParent(transform);
        bullet.transform.position = transform.position;
        for(int i = 0; i < SummonList.Count; i++)
        {
            if (SummonList[i] == null)
            {
                SummonList.RemoveAt(i);
            }
        }

    }
    public override void EnemySubAct()
    {
        if (SummonList.Count == 0)
        {
            for (int i = 0; i < summoner.VectorList.Count; i++)
            {
                GameObject Summon = Instantiate(summon);
                var trans = grid.WorldToCell(transform.position);
                Summon.transform.position =grid.GetCellCenterWorld( trans + summoner.VectorList[i]);
                //Summon.GetComponent<Enemy>().IsEnd = true;
                SummonList.Add(Summon);
            }
        }
    }

}
