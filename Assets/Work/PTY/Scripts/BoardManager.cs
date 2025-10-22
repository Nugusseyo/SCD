using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Grid boardTileGrid;
    
    public static BoardManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }
}
