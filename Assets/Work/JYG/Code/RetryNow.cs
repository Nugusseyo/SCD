using System.Linq;
using UnityEngine;
using Work.JYG.Code;
using YGPacks.PoolManager;

public class RetryNow : MonoBehaviour
{
    public void RetryRightNow()
    {
        EventManager.Instance.StopAllCoroutines();
        StatManager.Instance.ResetDatas();
        SaveManager.Instance.DeleteSave();
        StatManager.Instance.OnPriceChanged?.Invoke();
        StatManager.Instance.LoadMyValue();
        int listLength = EventManager.Instance.testEnemyList.Count;
        for (int i = listLength; i > 0; i--)
        {
            Destroy(EventManager.Instance.testEnemyList[i - 1].gameObject);
            EventManager.Instance.testEnemyList.RemoveAt(i - 1);
        }

        foreach (Piece player in EventManager.Instance.testPlayerList.ToList())
        {
            player.Die();
        EventManager.Instance.testEnemyList.Clear();

        EventManager.Instance.OnTurnChanged?.Invoke();
        }
        LifeDisplayer.Instance.OffMyUI();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                BoardManager.Instance.TileCompos[new Vector3(i, j)].SetOccupie(null);
            }
        }
        EventManager.Instance.testPlayerList.Clear();
    }
}
