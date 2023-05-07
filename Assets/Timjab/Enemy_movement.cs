using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_movement : MonoBehaviour
{
    public ILightObject Lighter_;
    Vector2 PositionOfMovement = new Vector2(0, 0);
    Vector2 playerPosition;
    [SerializeField]
    public float maxX = 10;
    [SerializeField]
    public float maxY = 4.5f;
    private void Awake()
    {
        PositionOfMovement = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
    }
    bool exceeded = false;
    bool moving = true;
    float lifeTime = 10;
    bool Passive = true;
    Vector2 Position = Vector2.zero;
    [SerializeField]
    float slowingSpeed = 1;
    [SerializeField]
    public float speed = 2;
    [SerializeField]
    float maxSpeed = 2;
    // Update is called once per frame
    float timePassed = 0;
    float sAttacktime = 0;
    bool lighted = false;
    bool scalingMovement = false;
    void AttackThePlayer()
    {
        lifeTime -= Time.deltaTime * 2;
        speed = 7.5f;
        PositionOfMovement = Lighter.mainLighter.transform.position;
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
    public void Scare()
    {
        lifeTime -= 5;
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
    public void Die()
    {
        Destroy(this.gameObject);
    }
    void SwitchPassive()
    {
        Passive = !Passive;
    }
    void Update()
    {
        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = lifeTime/15 + 0.2f;
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
        Position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        if (Lighter.mainLighter.GetComponent<Lighter>().Effects() && Vector2.Distance(Position,Lighter.mainLighter.gameObject.transform.position) < 5 && !lighted)
        {
            Scare();
        }
        if (Vector2.Distance(Position, CandleInformer.CandleLocation) < 5)
        {
            Destroy(gameObject);
        }
        else if(Vector2.Distance(Position,CandleInformer.CandleLocation) < 8) {
            Scare();
        }
        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
        playerPosition = Lighter.mainLighter.transform.position;
        if (!Passive) {
            AttackThePlayer();
        }
        else {
            lifeTime -= 1 * Time.deltaTime;
            speed = 2;
            timePassed += Time.deltaTime * 1;
            if(timePassed > Random.Range(2,6))
            {
                lighted = false;
                float probability = Random.Range(0.0f, 1.0f);
                if (probability <= 1 / (1 + Vector2.Distance(playerPosition, this.gameObject.transform.position)) && Vector2.Distance(playerPosition, this.gameObject.transform.position) < 5) {
                    if (!lighted) { Passive = false; }
                }
                else if (Vector2.Distance(Position, PositionOfMovement) < 0.5)
                {
                    PositionOfMovement = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
                    if(probability < 0.25f)
                    {
                        Multiplicate();
                    }
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