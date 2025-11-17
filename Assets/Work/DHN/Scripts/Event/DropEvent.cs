using Assets.Work.DHN.Scripts.Event;
using csiimnida.CSILib.SoundManager.RunTime;
using System.Collections.Generic;
using UnityEngine;
using YGPacks.PoolManager;

public abstract class  DropEvent : MonoBehaviour
{
    protected List<Meteo> SpawnItemList = new List<Meteo>();
    public int MeteoCount { get; private set; }

    protected Vector3 ItemDirection;

    protected Vector3 TilePos;

    public float Distance { get; private set; }

    public bool IsEnd { get; set; }

    [ContextMenu("메테오")]
    public void StartEvent(string itemName)
    {
        List<Vector3> targetPosList = new List<Vector3>();
        SpawnItemList.Clear();
        for (int i = 0; i < MeteoCount; i++)
        {
            IPoolable item = PoolManager.Instance.PopByName(itemName);
            Meteo meteo = item as Meteo;
            SpawnItemList.Add(meteo);
            if (BoardManager.Instance.TileCompos.TryGetValue(TilePos, out Tile tile))
            {
                TilePos = RandomTilePos();
                foreach (Vector3 pos in targetPosList)
                {
                    if (pos == TilePos)
                    {
                        //나중
                    }

                }
                tile = BoardManager.Instance.TileCompos[TilePos];
                meteo.targetPos = tile.transform.position; //나중가서 추가해야 할거
                meteo.transform.position = meteo.targetPos + (ItemDirection * Distance);
                SoundManager.Instance.PlaySound("Meteo");
            }
            //메테오를 가져왔따
        }
    }

    private Vector3 RandomTilePos()
    {
        return new Vector3(Random.Range(0, 8), Random.Range(0, 8), 0);
    }
}
