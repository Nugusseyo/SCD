using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code;
using Work.JYG.Code.Chessboard.Pieces;
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
        // -------------------------------
        // 1) 현재 보드에 있는 기물/적 싹 정리
        // -------------------------------
        ClearBoard();

        // -------------------------------
        // 2) 세이브 데이터 기준으로 다시 생성
        // -------------------------------
        LoadPieces(data.pieces);
        LoadEnemies(data.enemies);

        Debug.Log("[SaveLoadSystem] 로드 완료");
    }

    /// <summary>
    /// 보드 위의 모든 Piece / Enemy 제거 + 타일 점유 초기화
    /// </summary>
    private void ClearBoard()
    {
        var tileDict = BoardManager.Instance.TileCompos;

        foreach (var kv in tileDict)
        {
            Tile tile = kv.Value;
            if (tile.OccupiePiece == null) continue;

            GameObject go = tile.OccupiePiece;
            tile.SetOccupie(null);

            var piece = go.GetComponent<Piece>();
            var enemy = go.GetComponent<Enemy>();

            if (piece != null)
            {
                // 풀에 돌려보냄 (ResetItem 호출 포함)
                PoolManager.Instance.Push(piece);
            }
            else if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
            else
            {
                Destroy(go);
            }
        }

        // 이벤트 매니저 리스트도 비움
        EventManager.Instance.testPlayerList.Clear();
        EventManager.Instance.testEnemyList.Clear();
    }

    // ===========================
    //   Piece 복원
    // ===========================
    private void LoadPieces(List<PieceSaveData> list)
    {
        foreach (var ps in list)
        {
            // 풀에서 Piece 하나 뽑기
            var poolItem = PoolManager.Instance.PopByName("Piece");
            if (poolItem == null)
            {
                Debug.LogError("[SaveLoadSystem] Piece 풀에 아이템이 없습니다.");
                continue;
            }

            GameObject go = poolItem.GameObject;
            go.SetActive(true);
            go.transform.localScale = Vector3.one;

            // 스프라이트 강제 표시
            var renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var r in renderers)
            {
                r.enabled = true;
                var c = r.color;
                c.a = 1f;
                r.color = c;
            }

            Piece piece = go.GetComponent<Piece>();
            if (piece == null)
            {
                Debug.LogError("[SaveLoadSystem] 풀에서 꺼낸 오브젝트에 Piece 컴포넌트가 없습니다.");
                continue;
            }

            // 위치
            Vector3Int cell = ps.position;
            Vector3 worldPos = BoardGrid.GetCellCenterWorld(cell) + new Vector3(0, 0, -1f);
            piece.transform.position = worldPos;
            piece.curCellPos = cell;

            // 어떤 기물인지 (PieceSO)
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

            // 기본 이동 패턴 (VectorList)
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

                    AttributeSO attr = Resources.Load<AttributeSO>(attrName);
                    if (attr != null)
                        piece.Attributes.Add(attr);
                    else
                        Debug.LogWarning($"[SaveLoadSystem] AttributeSO 로드 실패: {attrName}");
                }
            }

            // Attribute 효과 적용 (추가 벡터 등)
            piece.OnAttributeChanged?.Invoke();

            // 체력 / 에너지 복원
            piece.CurrentHealth = Mathf.Clamp(ps.currentHealth, 0, piece.GetFinalMaxHealth());
            piece.CurrentEnergy = Mathf.Clamp(ps.currentEnergy, 0, piece.GetFinalMaxEnergy());
            piece.UpdateUI();

            // 🔹 해당 타일에 예전 기물이 남아있으면 먼저 정리하고, 지금 기물만 점유시키기
            if (BoardManager.Instance.TileCompos.TryGetValue((Vector3)cell, out Tile tile))
            {
                if (tile.OccupiePiece != null && tile.OccupiePiece != piece.gameObject)
                {
                    GameObject old = tile.OccupiePiece;
                    tile.SetOccupie(null);

                    var oldPiece = old.GetComponent<Piece>();
                    var oldEnemy = old.GetComponent<Enemy>();

                    if (oldPiece != null)
                        PoolManager.Instance.Push(oldPiece);
                    else if (oldEnemy != null)
                        Destroy(oldEnemy.gameObject);
                    else
                        Destroy(old);
                }

                tile.SetOccupie(piece.gameObject);
            }
            else
            {
                Debug.LogWarning($"[SaveLoadSystem] 해당 좌표에 타일이 없습니다: {cell}");
            }

            // 잡 상태 초기화
            piece.OnHold(false);
            piece.isSelected = false;

            // ★ 중요: PoolManager가 AppearanceItem 안에서 AddList 해준다면 이 줄은 빼도 됨
            // 중복 등록이 의심되면 아래 줄을 주석 처리해도 됨
            // EventManager.Instance.AddList(piece);
        }
    }

    // ===========================
    //   Enemy 복원
    // ===========================
    // SaveLoadSystem.LoadEnemies 부분만
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
            Vector3 worldPos = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(cell);

            Enemy enemy = Instantiate(prefab, worldPos, Quaternion.identity);
            enemy.gameObject.SetActive(true);

            if (enemy.grid == null)
                enemy.grid = BoardManager.Instance.boardTileGrid;

            enemy.CurrentHealth = es.currentHealth;

            // 스프라이트 강제 표시
            var renderers = enemy.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var r in renderers)
            {
                r.enabled = true;
                var c = r.color;
                c.a = 1f;
                r.color = c;
            }

            // 타일 점유
            if (BoardManager.Instance.TileCompos.TryGetValue(cell, out Tile tile))
            {
                tile.SetOccupie(enemy.gameObject);
            }

            // 에너미 리스트 등록
            if (!EventManager.Instance.testEnemyList.Contains(enemy))
                EventManager.Instance.testEnemyList.Add(enemy);
        }
    }

}
