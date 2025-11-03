using UnityEngine;

public class Tile : MonoBehaviour
{
    public Piece OccupiePiece { get; private set; } // ✅ Inspector에서 숨김 + 코드로만 설정

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        OccupiePiece = null;
    }

    public void SetOccupie(GameObject pieceObj)
    {
        OccupiePiece = pieceObj ? pieceObj.GetComponent<Piece>() : null;
    }

    public void ToggleSpriteRenderer()
    {
        if (_renderer != null)
            _renderer.enabled = !_renderer.enabled;
    }
}