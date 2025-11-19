using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyTest : MonoBehaviour
{
    private Grid grid;

    private void Start()
    {
        grid = BoardManager.Instance.boardTileGrid;
    }
    
    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Vector3Int slot = grid.WorldToCell(transform.position);
            BoardManager.Instance.TileCompos[slot].SetOccupie(gameObject);
        }
    }
}
