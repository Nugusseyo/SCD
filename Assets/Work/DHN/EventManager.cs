using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //싱글톤 선언 (static)
    //리스트 만들기 (IEvent 받는거)
    //메서드 - PlayerTurnSwap , bool _isPlayerTurn = False일때 PlayerTurnSwap
    //에너미 리스트 만들기 (에너미 Name = Enemy)
    //메서드 - 리스트에 에너미 넣어주기 (매개변수로 Enemy 받아오기)
    //메서드 - 리스트에서 에너미 빼주기 (매개변수로 Enemy 받아오기)
    public static EventManager Event { get; private set; }

    private bool _isPlayerTurn = true;

    List<Enemy> enemies = new List<Enemy>();
    List<IEvent>events = new List<IEvent>();

    public void AddEnemy(Enemy enemy) //추가 하는거
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy) //빼는거
    {
        enemies.Remove(enemy);
    }
    public void PlayerTurnSwap() //버튼 눌렀을떄
    {
        if(!_isPlayerTurn)
        {
            PlayerTurnSwap();
        }
    }

}
