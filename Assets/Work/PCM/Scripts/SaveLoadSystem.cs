using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code;
using Work.JYG.Code.Chessboard.Pieces;
using Work.PTY.Scripts;
using YGPacks.PoolManager;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance;

    [Header("Enemy Prefabs")]
    public List<Enemy> enemyPrefabs;

    private Grid BoardGrid => BoardManager.Instance.boardTileGrid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadAll()
    {
        FullSaveData data = SaveManager.Instance.LoadGame();

        if (data == null)
        {
            InitNewGame();
        }
        else
        {
            InitLoadGame(data);
        }
    }

    private void InitNewGame()
    {
        StatManager.Instance.ResetDatas();
        EventManager.Instance.TurnMyInput(true);
        Debug.Log("[SaveLoadSystem] 새 게임 시작");
    }

    private void InitLoadGame(FullSaveData data)
    {
        // ---------- 기존 말/적 정리 ----------

        // 플레이어 말들 → 보드 점유 해제 + 풀로 반환
        var playerList = new List<Piece>(EventManager.Instance.testPlayerList);
        foreach (var p in playerList)
        {
            if (p == null) continue;

            // 타일 점유 해제
            if (BoardManager.Instance.TileCompos.TryGetValue(p.curCellPos, out Tile tile) &&
                tile.OccupiePiece == p.gameObject)
            {
                tile.SetOccupie(null);
            }

            // 풀로 복귀
            PoolManager.Instance.Push(p);
        }
        EventManager.Instance.testPlayerList.Clear();

        // 적들 → 보드 점유 해제 + Destroy
        var enemyList = new List<Enemy>(EventManager.Instance.testEnemyList);
        foreach (var e in enemyList)
        {
            if (e == null) continue;

            if (e.grid != null)
            {
                Vector3Int cell = e.grid.WorldToCell(e.transform.position);
                if (BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile) &&
                    tile.OccupiePiece == e.gameObject)
                {
                    tile.SetOccupie(null);
                }
            }

            GameObject.Destroy(e.gameObject);
        }
        EventManager.Instance.testEnemyList.Clear();

        // 타일 점유 정보 전체 초기화 (혹시 남은 것 있으면)
        foreach (var kv in BoardManager.Instance.TileCompos)
            kv.Value.SetOccupie(null);

        // ---------- 새로 로드 ----------
        LoadPieces(data.pieces);
        LoadEnemies(data.enemies);

        Debug.Log("[SaveLoadSystem] 로드 완료");
    }

    // ---------- Piece 복원 ----------
    private void LoadPieces(List<PieceSaveData> list)
    {
        foreach (var ps in list)
        {
            var poolItem = PoolManager.Instance.PopByName("Piece");
            if (poolItem == null)
            {
                Debug.LogError("[SaveLoadSystem] Piece 풀에 아이템이 없습니다.");
                continue;
            }

            Piece piece = poolItem.GameObject.GetComponent<Piece>();
            if (piece == null)
            {
                Debug.LogError("[SaveLoadSystem] 풀에서 꺼낸 오브젝트에 Piece 컴포넌트가 없습니다.");
                continue;
            }

            Vector3Int cell = ps.position;
            Vector3 worldPos = BoardGrid.GetCellCenterWorld(cell) + new Vector3(0, 0, -1f);

            piece.transform.position = worldPos;
            piece.curCellPos = cell;

            // PieceData 복원 (PieceListSO 기준)
            if (ps.pieceIndex >= 0 &&
                ps.pieceIndex < StatManager.Instance.pieceList.pieces.Length)
            {
                piece.pieceData = StatManager.Instance.pieceList.pieces[ps.pieceIndex];
            }
            else
            {
                piece.pieceData = null;
                Debug.LogWarning($"[SaveLoadSystem] 잘못된 pieceIndex: {ps.pieceIndex}");
            }

            piece.SetData();

            // 기본 VectorList: PieceListSO 기준
            if (piece.pieceVectorLists == null)
                piece.pieceVectorLists = new List<ObjectVectorListSO>();
            else
                piece.pieceVectorLists.Clear();

            if (ps.pieceIndex >= 0 &&
                ps.pieceIndex < StatManager.Instance.pieceList.vectorLists.Length)
            {
                var baseVec = StatManager.Instance.pieceList.vectorLists[ps.pieceIndex];
                if (baseVec != null)
                    piece.pieceVectorLists.Add(baseVec);
            }

            // Attribute 복원
            if (piece.Attributes == null)
                piece.Attributes = new List<AttributeSO>();
            else
                piece.Attributes.Clear();

            if (ps.attributeNames != null && ps.attributeNames.Length > 0)
            {
                foreach (var attrName in ps.attributeNames)
                {
                    if (string.IsNullOrEmpty(attrName)) continue;

                    // Resources/attrName.asset 에 있다고 가정
                    AttributeSO attr = Resources.Load<AttributeSO>(attrName);
                    if (attr != null)
                        piece.Attributes.Add(attr);
                    else
                        Debug.LogWarning($"[SaveLoadSystem] AttributeSO 로드 실패: {attrName}");
                }
            }

            // Attribute 효과 재적용 (추가 VectorList 등)
            piece.OnAttributeChanged?.Invoke();

            // 체력/에너지 복원
            piece.CurrentHealth = Mathf.Clamp(ps.currentHealth, 0, piece.GetFinalMaxHealth());
            piece.CurrentEnergy = Mathf.Clamp(ps.currentEnergy, 0, piece.GetFinalMaxEnergy());
            piece.UpdateUI();

            // 🔹 타일에 자기 자신을 점유시켜서 클릭/공격에서 인식되도록
            if (BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile))
            {
                tile.SetOccupie(piece.gameObject);
            }
            else
            {
                Debug.LogWarning($"[SaveLoadSystem] 해당 좌표에 타일이 없습니다: {cell}");
            }

            piece.OnHold(false);
            piece.isSelected = false;

            EventManager.Instance.AddList(piece);
        }
    }

    // ---------- Enemy 복원 ----------
    private void LoadEnemies(List<EnemySaveData> list)
    {
        foreach (var es in list)
        {
            Enemy prefab = enemyPrefabs.Find(x => x.infos.name == es.enemySOName);
            if (prefab == null)
            {
                Debug.LogError($"[SaveLoadSystem] Enemy Prefab 찾기 실패: {es.enemySOName}");
                continue;
            }

            Vector3Int cell = es.position;
            Vector3 worldPos = BoardGrid.GetCellCenterWorld(cell);

            Enemy enemy = Object.Instantiate(prefab, worldPos, Quaternion.identity);
            enemy.CurrentHealth = es.currentHealth;

            if (BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile))
            {
                tile.SetOccupie(enemy.gameObject);
            }

            EventManager.Instance.AddList(enemy);
        }
    }
}
