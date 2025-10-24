using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TileChecker : MonoBehaviour
{
    private Piece _selPcCompo;
    private bool _isDragging = false;

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
        worldPos.z = 0;
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);

        if (touch.phase == TouchPhase.Began)
        {
            if (_isDragging && _selPcCompo != null)
            {
                _isDragging = false;

                Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos); 
                Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);

                bool moved = false;
                for (int i = 0; i < _selPcCompo.pieceVectorList.VectorList.Count; i++) 
                { 
                    Vector3Int moveableTile = _selPcCompo.curCellPos + _selPcCompo.pieceVectorList.VectorList[i];
                    if (BoardManager.Instance.tileCompos.ContainsKey(moveableTile))
                        BoardManager.Instance.tileCompos[moveableTile].ToggleSpriteRenderer();
                }
                
                for (int i = 0; i < _selPcCompo.pieceVectorList.VectorList.Count; i++) 
                { 
                    if (dropTile == _selPcCompo.curCellPos + _selPcCompo.pieceVectorList.VectorList[i]) 
                    { 
                        if (dropTile.x > 7 || dropTile.x < 0 || dropTile.y > 7 || dropTile.y < 0) 
                            break;

                        _selPcCompo.transform.position = cellCenter; 
                        _selPcCompo.curCellPos = dropTile; 
                        moved = true; 
                        Debug.Log("드롭 성공: 이동함");
                        
                        break;
                    }
                }
                
                if (!moved) 
                { 
                    _selPcCompo.transform.position = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(_selPcCompo.curCellPos);
                    Debug.Log("드롭 실패: 원위치 복귀");
                }

                if (dropTile.x <= 7 && dropTile.x >= 0 && dropTile.y <= 7 && dropTile.y >= 0) 
                    BoardManager.Instance.tileCompos[dropTile].setOccupie(_selPcCompo.gameObject);
                _selPcCompo.transform.Find("Visual").DOScale(1f, 0.3f).SetEase(Ease.OutBack); 
                _selPcCompo.isSelected = false; 
                _selPcCompo = null;
            }
            
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (hit != null)
            {
                _selPcCompo = hit.GetComponent<Piece>();
                if (!_isDragging && _selPcCompo != null)
                {
                    _selPcCompo.isSelected = true;
                    _isDragging = true;
                    hit.transform.DOKill();
                    hit.transform.Find("Visual").DOScale(2f, 0.3f).SetEase(Ease.OutBack);

                    Vector3Int emptyTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
                    BoardManager.Instance.tileCompos[emptyTile].setOccupie(null);

                    for (int i = 0; i < _selPcCompo.pieceVectorList.VectorList.Count; i++)
                    {
                        Vector3Int moveableTile = _selPcCompo.curCellPos + _selPcCompo.pieceVectorList.VectorList[i];
                        if (BoardManager.Instance.tileCompos.ContainsKey(moveableTile))
                            BoardManager.Instance.tileCompos[moveableTile].ToggleSpriteRenderer();
                    }

                    Debug.Log($"드래그 시작: {_selPcCompo.name}");
                }
            }
        }
    }
}
