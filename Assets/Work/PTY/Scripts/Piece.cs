using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class Piece : MonoBehaviour
{
    public PieceSO pieceData;
    public ObjectVectorListSO pieceVectorList;

    public Vector3Int curCellPos;

    public bool isSelected;

    private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        gameObject.name = pieceData.type.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = pieceData.sprite;
    }

    private void Start()
    {
        Vector3Int tilePos = BoardManager.Instance.boardTileGrid.WorldToCell(transform.position);
        curCellPos = tilePos;
    }

    private void Update()
    {
        // 선택된 상태일 때만 조작
        if (isSelected)
        {
            // 터치가 있을 때만 처리
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // 손가락이 화면 위에서 움직이는 동안 따라가게 함
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector3 worldPos =
                        Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    transform.position = worldPos;
                }
            }
        }
    }
}