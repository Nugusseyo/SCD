using DG.Tweening;
using UnityEngine;

public class TileCheck : MonoBehaviour
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

        switch (touch.phase)
        {
            case TouchPhase.Began:
                Collider2D hit = Physics2D.OverlapPoint(worldPos);

                if (hit != null)
                {
                    _selPcCompo = hit.GetComponent<Piece>();
                    if (_selPcCompo != null)
                    {
                        _selPcCompo.isSelected = true;
                        _isDragging = true;
                        hit.transform.DOKill();
                        hit.transform.Find("Visual").DOScale(2f, 0.3f).SetEase(Ease.OutBack);
                        Debug.Log($"드래그 시작: {_selPcCompo.name}");
                    }
                }
                break;
            case TouchPhase.Canceled:
                if (_isDragging && _selPcCompo != null)
                {
                    _isDragging = false;

                    // 터치가 끝난 위치의 타일 확인
                    Vector3Int dropTile = BoardManager.Instance.boardTileGrid.WorldToCell(worldPos);
                    Vector3 cellCenter = BoardManager.Instance.boardTileGrid.GetCellCenterWorld(dropTile);

                    bool moved = false;
                    for (int i = 0; i < _selPcCompo.pieceVectorList.VectorList.Count; i++)
                    {
                        if (dropTile == _selPcCompo.curCellPos + _selPcCompo.pieceVectorList.VectorList[i])
                        {
                            _selPcCompo.transform.position = cellCenter;
                            _selPcCompo.curCellPos = dropTile;
                            moved = true;
                            Debug.Log("드롭 성공: 이동함");
                            break;
                        }
                    }

                    // 이동 불가한 경우 원위치 복귀
                    if (!moved)
                    {
                        _selPcCompo.transform.position =
                            BoardManager.Instance.boardTileGrid.GetCellCenterWorld(_selPcCompo.curCellPos);
                        Debug.Log("드롭 실패: 원위치 복귀");
                    }

                    // 선택 해제
                    _selPcCompo.transform.Find("Visual").DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                    _selPcCompo.isSelected = false;
                    _selPcCompo = null;
                }
                break;
        }
    }
}
