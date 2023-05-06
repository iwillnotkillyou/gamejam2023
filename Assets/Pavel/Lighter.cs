using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface ILightObject
{
    public Vector3 Pos { get; }

    public bool Effects();
}

public class Lighter : MonoBehaviour, ILightObject
{
    public Sprite LighterSprite;
    private float buffDuration = 0;
    private float buffCD = 0;
    private int brokenHits = 0;
    public bool Yeet;
    public float YeetRadius = 1;

    public void TurnIntoLighter()
    {
        GetComponent<SpriteRenderer>().sprite = LighterSprite;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buffCD -= Time.deltaTime;


        if (Input.GetMouseButtonDown(1))
        {
            if (buffCD < 0 && brokenHits <= 0)
            {
                transform.GetChild(2).gameObject
                    .GetComponent<Light2D>().intensity = 2;
                buffDuration = 2f;
                buffCD = 5f;
            }

            if (brokenHits > 0)
            {
                brokenHits--;
                //sound effect and spark effect
            }
        }

        if (buffCD < 0)
        {
            transform.GetChild(2).gameObject
                .GetComponent<Light2D>().intensity = 1f;
        }

        if (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
        }
        else if (buffDuration <= 0)
        {
            transform.GetChild(2).gameObject
                .GetComponent<Light2D>().intensity = 0.5f;
            brokenHits = Mathf.Max(0,Random.Range(0, 10) - 5);
        }
    }

    public Vector3 Pos => transform.position;
    public float Range => YeetRadius;
    public bool Effects()
    {
        var lDir = (Pos - transform.position);
        return (lDir.magnitude < Range) && Yeet;
    }
}
