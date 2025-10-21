using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoardMaker : MonoBehaviour
{
    public List<Vector3Int> tileList = new List<Vector3Int>();

    public GameObject prefab;

    [SerializeField] private GridLayout grid;

    private void Awake()
    {
        grid = GetComponent<GridLayout>();
    }

    private void Start()
    {
        foreach (Vector3Int move in tileList)
        {
            GameObject tile = Instantiate(prefab, transform);
            tile.transform.position = grid.CellToWorld(move);
        }
    }
}
