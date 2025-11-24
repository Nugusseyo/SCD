
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]private EnemysSO enemySo;
    [SerializeField]private int enemySpawntime;
    public Grid grid;
    private Vector3Int cell;

    private void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
        enemySpawntime = enemySo.Spawning;
    }
    public void SpawnTime()
    {
        cell = grid.WorldToCell(transform.position);
        cell.y = 7;

        if (BoardManager.Instance.TileCompos[cell].OccupiePiece == null)
            enemySpawntime -= 1;
        if(enemySpawntime <= 0) 
        {
            gameObject.GetComponent<Enemy>().enabled = true;
            gameObject.GetComponent<Enemy>().IsEnd = true;
        } 
    }
}