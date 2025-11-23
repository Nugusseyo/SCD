using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Work.JYG.Code;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(List<Piece> pieces, List<Enemy> enemies)
    {
        var data = new FullSaveData();

        // ---------- Piece 저장 ----------
        foreach (var p in pieces)
        {
            if (p == null) continue;

            // 1) pieceData 없는 껍데기는 저장 안 함
            if (p.pieceData == null) continue;

            // 2) 실제 보드 위에 올라가 있는 애만 저장
            if (!BoardManager.Instance.TileCompos.TryGetValue(p.curCellPos, out Tile tile)) continue;
            if (tile.OccupiePiece != p.gameObject) continue;

            var ps = new PieceSaveData
            {
                position = p.curCellPos,
                currentHealth = p.CurrentHealth,
                currentEnergy = p.CurrentEnergy,
                pieceIndex = p.pieceData.pieceIndex
            };

            // Attribute 이름 저장
            if (p.Attributes != null && p.Attributes.Count > 0)
            {
                var attrNames = new List<string>();
                foreach (var attr in p.Attributes)
                {
                    if (attr != null)
                        attrNames.Add(attr.name);
                }

                ps.attributeNames = attrNames.ToArray();
            }

            data.pieces.Add(ps);
        }

        // ---------- Enemy 저장 ----------
        foreach (var e in enemies)
        {
            if (e == null) continue;

            if (e.grid == null)
                e.grid = FindAnyObjectByType<Grid>();

            Vector3Int cell = e.grid.WorldToCell(e.transform.position);

            // 적도 보드 위에 있는 애만
            if (!BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile)) continue;
            if (tile.OccupiePiece != e.gameObject) continue;

            var es = new EnemySaveData
            {
                enemySOName = e.infos.name,
                position = cell,
                currentHealth = e.CurrentHealth
            };

            data.enemies.Add(es);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] 저장 완료: {SavePath}");
    }

    public FullSaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveManager] 저장 파일 없음");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<FullSaveData>(json);
    }

    [ContextMenu("Delete")]
    public void DeleteSave()
    {
        if (!File.Exists(SavePath)) return;

        File.Delete(SavePath);
        Debug.Log("[SaveManager] 세이브 삭제 완료");
    }
}
