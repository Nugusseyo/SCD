using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class TileChecker : MonoBehaviour
{
    public Vector3 dragOffset;
    
    private Piece _selPcCompo;
    private bool _pieceSelected = false;
    private List<Vector3Int> _highlightedTiles = new List<Vector3Int>();

    void Update()
    {
        if (Input.touchCount == 0) return;

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

    private void OnTouchBegan(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        Piece piece = hit ? hit.GetComponent<Piece>() : null;

        if (piece != null && !_pieceSelected)
            SelectPiece(piece, worldPos);
    }

    private void OnTouchMoved(Vector3 worldPos)
    {
        if (_pieceSelected && _selPcCompo != null)
            _selPcCompo.transform.position = worldPos + new Vector3(0, 0, 9) + dragOffset;
    }

    private void OnTouchEnded(Vector3 worldPos)
    {
        if (_pieceSelected)
            TryMoveToTile(worldPos);
        else if (_selPcCompo != null)
            _pieceSelected = true;
    }

    private void SelectPiece(Piece piece, Vector3 worldPos)
    {
        if (_selPcCompo != null)
            ClearHighlight();

        _selPcCompo = piece;
        _selPcCompo.isSelected = true;
        _selPcCompo.transform.DOKill();
        _selPcCompo.transform.Find("Visual").DOScale(2f, 0.3f).SetEase(Ease.OutBack);
        _selPcCompo.OnHold();

        Vector3Int curTile = _selPcCompo.curCellPos;
        BoardManager.Instance.tileCompos[curTile].SetOccupie(null);
        
        _highlightedTiles.Clear();

        foreach (var moveVector in _selPcCompo.pieceVectorList.VectorList)
        {
            Vector3Int moveableTile = curTile + moveVector;
            if (BoardManager.Instance.tileCompos.ContainsKey(moveableTile))
            {
                var highlightableTile = BoardManager.Instance.tileCompos[moveableTile];
                if (highlightableTile.GetComponent<Tile>().OccupiePiece == null)
                {
                    highlightableTile.ToggleSpriteRenderer();
                    _highlightedTiles.Add(moveableTile);
                }
            }
        }

        Debug.Log($"기물 선택: {_selPcCompo.name}");
    }

    private void TryMoveToTile(Vector3 worldPos)
    {
        if (_selPcCompo == null) return;
        
        Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
        Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);
        bool moved = false;

        if (BoardManager.Instance.tileCompos[dropTile].GetComponent<SpriteRenderer>().enabled)
        {
            _selPcCompo.transform.position = cellCenter + new Vector3(0, 0, -1);
            _selPcCompo.curCellPos = dropTile;
            moved = true;
            Debug.Log("이동 성공");
        }
        else
        {
            _selPcCompo.transform.position =
                BoardManager.Instance.boardTileGrid.GetCellCenterWorld(_selPcCompo.curCellPos) + new Vector3(0, 0, -1);
            BoardManager.Instance.tileCompos[_selPcCompo.curCellPos].SetOccupie(_selPcCompo.gameObject);
            Debug.Log("이동 실패: 원위치 복귀");
        }

        ClearHighlight();

        if (moved && dropTile.x >= 0 && dropTile.x < 8 && dropTile.y >= 0 && dropTile.y < 8)
            BoardManager.Instance.tileCompos[dropTile].SetOccupie(_selPcCompo.gameObject);

        _selPcCompo.transform.Find("Visual").DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        _selPcCompo.isSelected = false;
        _selPcCompo.OnHold();
        _selPcCompo = null;
        _pieceSelected = false;
    }

    private void ClearHighlight()
    {
        foreach (var tilePos in _highlightedTiles)
        {
            if (BoardManager.Instance.tileCompos.ContainsKey(tilePos))
            {
                var tile = BoardManager.Instance.tileCompos[tilePos];
                if (tile.GetComponent<Tile>().OccupiePiece == null)
                    tile.ToggleSpriteRenderer();
            }
        }
        _highlightedTiles.Clear();
    }
}
