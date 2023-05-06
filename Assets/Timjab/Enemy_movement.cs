using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_movement : MonoBehaviour
{
    [SerializeField]
    float maxX = 10;
    [SerializeField]
    float maxY = 4.5f;
    private void Awake()
    {
        this.transform.position = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
    }
    bool exceeded = false;
    bool moving = true;
    bool Passive = true;
    Vector2 Position = Vector2.zero;
    [SerializeField]
    Vector2 PositionOfMovement = new Vector2(0, 0);
    [SerializeField]
    float slowingSpeed = 1;
    [SerializeField]
    float speed = 2;
    [SerializeField]
    float maxSpeed = 2;
    // Update is called once per frame
    float timePassed = 0;
    float sAttacktime = 0;
    bool lighted = false;
    bool scalingMovement = false;
    void AttackThePlayer()
    {
        speed = 7.5f;
        PositionOfMovement = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sAttacktime += Time.deltaTime * 1;
        if(sAttacktime >= Random.Range(5,20))
        {
            sAttacktime = 0;
            scalingMovement = true;
        }

    }
    void Multiplicate()
    {
        GameObject DemonChild = Instantiate(this.gameObject);
        DemonChild.transform.position = this.gameObject.transform.position;
    }
    void Scare()
    {
        lighted = true;
        Passive = true;
        PositionOfMovement = (-1)*PositionOfMovement; 
    }
    private void OnDestroy()
    {
        //animation and sound
        //after its done:
        Destroy(this.gameObject);
    }
    void SwitchPassive()
    {
        Passive = !Passive;
    }
    void Update()
    {
        Vector2 playerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        if (!Passive) {
            AttackThePlayer();
        }
        else {
            speed = 2;
            timePassed += Time.deltaTime * 1;
            if(timePassed > Random.Range(2,6))
            {
                float probability = Random.Range(0.0f, 1.0f);
                if (probability <= 1 / (1 + Vector2.Distance(playerPosition,this.gameObject.transform.position)) && Vector2.Distance(playerPosition,this.gameObject.transform.position) < 5) {
                    if (!lighted) { Passive = false; }
                }
                else if(Vector2.Distance(Position, PositionOfMovement) < 0.5)
                {
                    Multiplicate();
                    PositionOfMovement = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
                }
            }
        }

        if (Vector2.Distance(Position, PositionOfMovement) < 0.5) {
            if (this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
            {
                SlowDown();
            }
            else { this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); }
        }
        else { MoveToPosition(); }
    }
    void MoveToPosition()
    {
        if (scalingMovement) {
            Vector2 movementVector = PositionOfMovement - Position;
            this.gameObject.GetComponent<Rigidbody2D>().velocity += speed * Time.deltaTime * movementVector.normalized;  
            
            if(exceeded)
            {
                if(Vector2.Distance(PositionOfMovement,this.gameObject.transform.position) > 2)
                {
                    exceeded = false;
                    scalingMovement = false;
                }
            }
            else
            {
                if(Vector2.Distance(PositionOfMovement, this.gameObject.transform.position) < 2)
                {
                    exceeded=true;
                }
            }
        }
        else {
            Vector2 movementVector = PositionOfMovement - Position;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = speed * movementVector.normalized;
        }
    }
    void SlowDown()
    {
            this.gameObject.GetComponent<Rigidbody2D>().velocity += -speed * Time.deltaTime * this.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }
}