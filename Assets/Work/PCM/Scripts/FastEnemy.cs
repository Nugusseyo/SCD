using UnityEngine;
using Work.JYG.Code;

public class FastEnemy : Enemy
{
    [SerializeField] private GameObject Slash;
    public override void EnemySpcAct()
    {
        for(int i = 0; i < attackResult.Count; i++) 
        {
            GameObject slash = Instantiate(Slash);        
            slash.transform.position =grid.GetCellCenterWorld(attackResult[i]);
            attack.FastEnemyAttack(infos.EnemyStat.attack);

        }
    }
    public override void Die()
    {
        PlayerPrefs.SetInt("BossDie", PlayerPrefs.GetInt("BossDie") + 1);
        ChallengeManager.Instance.OnChallengeSwitchContacted?.Invoke();
    }
}
