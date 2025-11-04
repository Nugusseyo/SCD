using UnityEngine;

public interface ITurnAble
{
    public int MaxEnergy {get; set;}
    public int CurrentEnergy {get; set;}
    public bool IsEnd { get; set;}

}
