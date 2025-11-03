using System;
using System.Collections.Generic;
using UnityEngine;

public class TestMeteo : MonoBehaviour
{
    public Vector2 Pos = new Vector2(-1, -1);
    public float speed = 5f;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

        rigid.linearVelocity = Pos.normalized * speed;
    }
}
