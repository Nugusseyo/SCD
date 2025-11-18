using Unity.VisualScripting;
using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]private EnemysSO enemySo;
    [SerializeField]private int enemySpawntime;
    private void Awake()
    {
        enemySpawntime = enemySo.Spawning;
    }
    public void SpawnTime()
    {
        enemySpawntime -= 1;
        if(enemySpawntime <= 0) 
        {
            Debug.Log("ada");
            gameObject.GetComponent<TestEnemyScrip>().enabled = true;
        }
    }
}
