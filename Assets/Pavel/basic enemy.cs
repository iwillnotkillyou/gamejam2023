using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class basicenemy : MonoBehaviour
{
    public ILightObject Lighter;
    public ILightObject Candle;
    private float startTime;
    private int moveDir = -1;

    public Vector2 dir = new Vector2(Random.Range(-1f, 1f),
        Random.Range(-1f, 1f))*10;

    void Start()
    {
        startTime = Time.time;
        GetComponent<Rigidbody2D>().velocity = moveDir*dir;
    }

    // Update is called once per frame
    void Update()
    {
        var lDir = (Lighter.Pos - transform.position);
        if (lDir.magnitude <
            Lighter.Range)
        {
            GetComponent<Rigidbody2D>().velocity = lDir.normalized *
                GetComponent<Rigidbody2D>().velocity.magnitude;
        }

        if (Mathf.Abs(startTime - Time.time) > 1)
        {
            moveDir *= -1;
            GetComponent<Rigidbody2D>().velocity = moveDir*dir;
        }

        startTime = Time.time;
    }
}
