using UnityEngine;
using Work.JYG.Code;

public class DamageBigEnemy : Enemy
{
    [SerializeField]private GameObject Slash;
    public override void EnemySpcAct()
    {
        GameObject slash = Instantiate(Slash);
        
        slash.transform.position = new Vector2(transform.position.x,transform.position.y-0.2f);
        attack.FastEnemyAttack(infos.EnemyStat.attack);

    }
    public override void Die()
    {
        PlayerPrefs.SetInt("BossDie", PlayerPrefs.GetInt("BossDie") + 1);
        ChallengeManager.Instance.OnChallengeSwitchContacted?.Invoke();
    }
}
