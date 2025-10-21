using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private int movetile = 3;
    List<Vector2> move = new List<Vector2>();
    Dictionary<Vector2 , int> moveValue = new Dictionary<Vector2 , int>();
    private Ray ray;
    [SerializeField] private LayerMask unit;
  public void Brain(Vector3 moves)
  {
     move.Clear();
     moveValue.Clear();

     for (int i = 0; i < move.Count; i++) 
     {
            Ray ray = new Ray(move[i], move[i]);
            RaycastHit[] hit = Physics.RaycastAll(ray, 30f, unit);
            Debug.Log($"key{i} value{hit.Length}");
            moveValue.Add(move[i] ,hit.Length);
     }
      transform.position = moveValue.OrderByDescending(x => x.Value).First().Key;
  }
}
