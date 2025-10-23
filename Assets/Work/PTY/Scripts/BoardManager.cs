using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Grid boardTileGrid;
    public GameObject slotPrefab;

    public Dictionary<Vector3, Tile> tileCompos = new();

    public static BoardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        // 8x8 보드 생성
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                Vector3 worldPos = boardTileGrid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                GameObject slot = Instantiate(slotPrefab, worldPos, Quaternion.identity);
                slot.transform.SetParent(boardTileGrid.transform, worldPositionStays: true);
                slot.transform.localScale = boardTileGrid.cellSize;
                slot.name = $"Slot_{x}_{y}";
                slot.GetComponent<SpriteRenderer>().enabled = false;
                Tile tileCompo = slot.GetComponent<Tile>();
                tileCompos.Add(new Vector3Int(x, y, 0), tileCompo);
            }
        }
    }
}