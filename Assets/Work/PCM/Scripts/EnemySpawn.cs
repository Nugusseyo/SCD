using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

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
    private void Start()
    {
        cell = grid.WorldToCell(transform.position);
        cell.y = 7;
    }
    public void SpawnTime()
    {
        
        if (BoardManager.Instance.TileCompos[cell].OccupiePiece == null)
            enemySpawntime -= 1;
        if(enemySpawntime <= 0) 
        {
            gameObject.GetComponent<Enemy>().enabled = true;
            gameObject.GetComponent<Enemy>().IsEnd = true;
        } 
    }
}
