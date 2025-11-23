using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject OccupiePiece { get; private set; }

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        OccupiePiece = null;
    }

    public void SetOccupie(GameObject pieceObj)
    {
        OccupiePiece = pieceObj;
    }

    public void ToggleSpriteRenderer(bool toggle)
    {
        _renderer.enabled = toggle;
    }
}