using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface ILightObject
{
    public Vector3 Pos { get; }

    public bool Effects();
}
public class Lighter : MonoBehaviour, ILightObject
{
    public static GameObject mainLighter;
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
    void Awake()
    {
        mainLighter = gameObject;
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
                transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                //sound effect and spark effect
            }
        }

        if (brokenHits > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject
                .GetComponent<Light2D>().intensity = 0.1f;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }

        if (buffCD < 0 && brokenHits < 0)
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
        return buffDuration > 0;
    }
}
