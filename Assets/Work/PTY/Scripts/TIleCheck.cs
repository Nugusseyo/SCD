using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class TIleCheck : MonoBehaviour
{
    public Grid boardTileGrid;

    private Piece _selectedPieceComponenet;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector3Int tilePos = boardTileGrid.WorldToCell(worldPos);

            if (tilePos.x >= 0 && tilePos.x < 8 && tilePos.y >= 0 && tilePos.y < 8)
            {
                Vector3 cellCenter = boardTileGrid.GetCellCenterWorld(tilePos);
                Collider2D hit = Physics2D.OverlapPoint(cellCenter);

                if (hit != null)
                {
                    Debug.Log($"{tilePos} 위에 {hit.gameObject.name} 이(가) 있습니다.");
                    if (_selectedPieceComponenet == null)
                        _selectedPieceComponenet = hit.gameObject.GetComponent<Piece>();
                    
                    if (!_selectedPieceComponenet.isSelected)
                    {
                        hit.gameObject.transform.DOKill();
                        hit.gameObject.transform.Find("Visual").DOScale(2f, 1f).SetEase(Ease.OutCirc);
                        _selectedPieceComponenet.isSelected = true;
                    }
                    else
                    {
                        hit.gameObject.transform.DOKill();
                        hit.gameObject.transform.Find("Visual").DOScale(1f, 1f).SetEase(Ease.OutCirc);
                        _selectedPieceComponenet.isSelected = false;
                        hit.gameObject.transform.position = cellCenter;
                    }
                }
                else
                    Debug.Log($"{tilePos} 위에는 아무것도 없습니다.");
            }
        }
    }
}
