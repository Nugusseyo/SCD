using UnityEngine;
using Work.JYG.Code.Chessboard.Pieces;

public class ShotEnemy : Enemy
{
    [SerializeField]private GameObject bullet;
    public override void EnemySpcAct()
    {
        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.SetParent(transform);
        if (CompareTag("Boss"))
        {
            Vector3Int IntTrans = attackResult[0]; 
            bullet.transform.position =grid.GetCellCenterWorld(IntTrans);
        }
        else
        {
            bullet.transform.position = transform.position;
        }
    }
}
