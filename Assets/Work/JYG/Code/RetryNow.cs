using System.Linq;
using UnityEngine;
using Work.JYG.Code;
using YGPacks.PoolManager;

public class RetryNow : MonoBehaviour
{
    public void RetryRightNow()
    {
        StatManager.Instance.ResetDatas();
        SaveManager.Instance.DeleteSave();
        StatManager.Instance.OnPriceChanged?.Invoke();
        StatManager.Instance.LoadMyValue();
        foreach (Enemy enemy in EventManager.Instance.testEnemyList.ToList())
        {
            Destroy(enemy);
        }

        EventManager.Instance.testEnemyList.Clear();
        foreach (Piece player in EventManager.Instance.testPlayerList.ToList())
        {
            player.Die();
        }
        EventManager.Instance.testPlayerList.Clear();

        EventManager.Instance.OnTurnChanged?.Invoke();
        LifeDisplayer.Instance.OffMyUI();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                BoardManager.Instance.TileCompos[new Vector3(i, j)].SetOccupie(null);
            }
        }
    }
}
