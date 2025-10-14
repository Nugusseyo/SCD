using UnityEngine;

public class Board : MonoBehaviour
{
    private const int BOARD_SIZE_X = 8;
    private const int BOARD_SIZE_Y = 8;
    public GameObject slot;

    private void Awake()
    {
        InitBoard();
    }

    private void InitBoard()
    {
        for(int i = 0; i < BOARD_SIZE_X; i++)
            for (int j = 0; j < BOARD_SIZE_Y; j++)
            {
                GameObject generatedSlot = Instantiate(slot, Vector3.zero, Quaternion.identity);
                generatedSlot.transform.SetParent(transform);
            }
                
    }
}
