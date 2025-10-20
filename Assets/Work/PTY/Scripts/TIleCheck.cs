using UnityEngine;
using UnityEngine.InputSystem;

public class TIleCheck : MonoBehaviour
{
    public Grid boardTileGrid;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector3Int tilePos = boardTileGrid.WorldToCell(worldPos);

            Vector3 cellCenter = boardTileGrid.GetCellCenterWorld(tilePos);
            Collider2D hit = Physics2D.OverlapPoint(cellCenter);

            if (hit != null)
                Debug.Log($"{tilePos} 위에 {hit.gameObject.name} 이(가) 있습니다.");
            else
                Debug.Log($"{tilePos} 위에는 아무것도 없습니다.");
        }

    }
}
