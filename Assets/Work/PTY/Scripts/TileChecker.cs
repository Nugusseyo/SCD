using System.Collections;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using csiimnida.CSILib.SoundManager.RunTime;
using Work.PTY.Scripts.PieceManager;
using YGPacks;
using Vector3 = UnityEngine.Vector3;

public class TileChecker : Singleton<TileChecker>
{
    public Vector3 dragOffset;
    public float shakeAmount = 5f;
    
    public Piece SelPcCompo { get; private set; }
    private bool _pieceSelected = false;
    private List<Vector3Int> _highlightedTiles = new List<Vector3Int>();

    private bool IsInput => EventManager.Instance.debugIsOk;

    private Coroutine _shakeCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    private void Update()
    {
        if (Input.touchCount == 0) return;
        if (PieceManager.Instance.isPlacingPiece) return;

        if (IsInput)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(worldPos);
                    break;

                case TouchPhase.Moved:
                    OnTouchMoved(worldPos);
                    break;

                case TouchPhase.Ended:
                    OnTouchEnded(worldPos);
                    break;
            }
        }
    }

    private void OnTouchBegan(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        Piece piece = hit ? hit.GetComponent<Piece>() : null;

        if (piece != null && !_pieceSelected)
            SelectPiece(piece, worldPos);
    }

    private void OnTouchMoved(Vector3 worldPos)
    {
        if (_pieceSelected && SelPcCompo != null)
            SelPcCompo.transform.position = worldPos + new Vector3(0, 0, 9) + dragOffset;
    }

    private void OnTouchEnded(Vector3 worldPos)
    {
        if (_pieceSelected)
            TryMoveToTile(worldPos);
        else if (SelPcCompo != null)
            _pieceSelected = true;
    }
    
    private IEnumerator ShakePiece(Transform visual)
    {
        float targetZ = shakeAmount;
        while (true)
        {
            targetZ *= -1;
            float duration = 0.25f;

            visual.DOLocalRotate(new Vector3(0, 0, targetZ), duration).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(duration);
        }
    }

    private void SelectPiece(Piece piece, Vector3 worldPos)
    {
        
        if (SelPcCompo != null)
            ClearHighlight();

        SelPcCompo = piece;

        if (PieceManager.Instance.IsAttacking)
        {
            Debug.LogWarning("공격중임");
            return;
        }
        
        SelPcCompo.isSelected = true;
        SelPcCompo.transform.DOKill();
        SelPcCompo.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack);
        SelPcCompo.OnHold(true);

        _shakeCoroutine = StartCoroutine(ShakePiece(SelPcCompo.GetComponentInChildren<SpriteRenderer>().transform));

        Vector3Int curTile = SelPcCompo.curCellPos;
        BoardManager.Instance.TileCompos[curTile].SetOccupie(null);
        
        _highlightedTiles.Clear();

        foreach (var pieceVectorList in SelPcCompo.pieceVectorLists)
        {
            foreach (var moveVector in pieceVectorList.VectorList)
            {
                Vector3Int moveableTile = curTile + moveVector;
                if (BoardManager.Instance.TileCompos.ContainsKey(moveableTile))
                {
                    var highlightableTile = BoardManager.Instance.TileCompos[moveableTile];
                    if (highlightableTile.GetComponent<Tile>().OccupiePiece == null)
                    {
                        highlightableTile.ToggleSpriteRenderer(true);
                        _highlightedTiles.Add(moveableTile);
                    }
                }
            }

            SoundManager.Instance.PlaySound("PiecePick");
            Debug.Log($"기물 선택: {SelPcCompo.name}"); 
        }
    }

    private void TryMoveToTile(Vector3 worldPos)
    {
        if (SelPcCompo == null) return;
    
        StopCoroutine(_shakeCoroutine);
        SelPcCompo.GetComponentInChildren<SpriteRenderer>().transform.DORotate(Vector3.zero, 0.5f);
        
        Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
        Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);
        bool moved = false;

        if (!BoardManager.Instance.TileCompos.ContainsKey(dropTile))
        {
            Debug.LogWarning($"보드 범위 밖 타일 접근 시도: {dropTile}");
            SelPcCompo.transform.position =
                BoardManager.Instance.boardTileGrid.GetCellCenterWorld(SelPcCompo.curCellPos) + new Vector3(0, 0, -1);
            BoardManager.Instance.TileCompos[SelPcCompo.curCellPos].SetOccupie(SelPcCompo.gameObject);
            SelPcCompo.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            SelPcCompo.isSelected = false;
            SelPcCompo.OnHold(false);
            SelPcCompo = null;
            _pieceSelected = false;
            ClearHighlight();
            return;
        }

        SpriteRenderer spriteRenderer = BoardManager.Instance.TileCompos[dropTile].GetComponent<SpriteRenderer>();

        if (spriteRenderer.enabled)
        {
            if (SelPcCompo.CurrentEnergy <= 0)
            {
                SelPcCompo.transform.position =
                    BoardManager.Instance.boardTileGrid.GetCellCenterWorld(SelPcCompo.curCellPos) + new Vector3(0, 0, -1);
                BoardManager.Instance.TileCompos[SelPcCompo.curCellPos].SetOccupie(SelPcCompo.gameObject);
                SelPcCompo.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                SelPcCompo.isSelected = false;
                SelPcCompo.OnHold(false);
                Debug.LogWarning($"{SelPcCompo.name}의 에너지 부족함!");
                SelPcCompo = null;
                _pieceSelected = false;
                ClearHighlight();
                return;
            }
            else
            {
                SelPcCompo.transform.position = cellCenter + new Vector3(0, 0, -1);
                SelPcCompo.curCellPos = dropTile;
                moved = true;
                Debug.Log("이동 성공");
            }
        }
        else
        {
            SelPcCompo.transform.position =
                BoardManager.Instance.boardTileGrid.GetCellCenterWorld(SelPcCompo.curCellPos) + new Vector3(0, 0, -1);
            BoardManager.Instance.TileCompos[SelPcCompo.curCellPos].SetOccupie(SelPcCompo.gameObject);
            Debug.LogWarning($"이동 실패: {dropTile}, 원위치 복귀");
        }

        ClearHighlight();

        if (moved && dropTile.x >= 0 && dropTile.x < 8 && dropTile.y >= 0 && dropTile.y < 8)
        {
            BoardManager.Instance.TileCompos[dropTile].SetOccupie(SelPcCompo.gameObject);
            SelPcCompo.ReduceEnergy(1);
        }

        SelPcCompo.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        SelPcCompo.isSelected = false;
        SelPcCompo.OnHold(false);
        SelPcCompo = null;
        _pieceSelected = false;
        SoundManager.Instance.PlaySound("PiecePick");
    }


    private void ClearHighlight()
    {
        foreach (var tilePos in _highlightedTiles)
        {
            if (BoardManager.Instance.TileCompos.ContainsKey(tilePos))
            {
                var tile = BoardManager.Instance.TileCompos[tilePos];
                if (tile.GetComponent<Tile>().OccupiePiece == null)
                    tile.ToggleSpriteRenderer(false);
            }
        }
        _highlightedTiles.Clear();
    }
}
