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
    public bool active = false;


    public void TurnIntoLighter()
    {
        active = true;
        GetComponent<SpriteRenderer>().sprite = LighterSprite;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        buffDuration = 0;
        buffCD = 0;
        brokenHits = 0;
        YeetRadius = 1;
        active = false;
        mainLighter = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active)
        {
            return;
        }

        buffCD -= Time.fixedDeltaTime;


        if (Input.GetMouseButtonDown(1))
        {
            if (buffCD < 0 && brokenHits <= 0)
            {
                transform.GetChild(2).gameObject
                    .GetComponent<Light2D>().intensity = 2;
                buffDuration = 2f;
                buffCD = 8f;
                brokenHits = Mathf.Max(0,Random.Range(0, 10) - 3);
            }

            if (brokenHits > 0 && buffDuration < 0)
            {
                brokenHits--;
                transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                //sound effect and spark effect
            }
        }

        if (buffDuration > 0)
        {
            buffDuration -= Time.fixedDeltaTime;
            transform.GetChild(0).gameObject.SetActive(false);
            if (!transform.GetChild(3).gameObject.activeInHierarchy)
            {
                transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        
        if (buffDuration < 0 && brokenHits <= 0)
        {
            transform.GetChild(3).gameObject.SetActive(false);
            print("c");
            if (!transform.GetChild(0).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }

            transform.GetChild(2).gameObject.GetComponent<Light2D>()
                .intensity = buffCD > 0 ? 0.5f : 1f;
        }

        if (buffDuration < 0 && brokenHits > 0)
        {
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject
                .GetComponent<Light2D>().intensity = 0.1f;
        }
    }

    public Vector3 Pos => transform.position;
    public float Range => YeetRadius;
    public bool Effects()
    {
        return buffDuration > 0;
    }
}
