using UnityEngine;

public class TestPlayerStat : MonoBehaviour
{
    [SerializeField]private AgentStatSO statSO;

    [field:SerializeField]public int Attack { get; set; }
    [field:SerializeField]public int Hp {  get;set; }    
    private void Awake()
    {
        Hp = statSO.hp;
        Attack = statSO.attack;
    }


}
