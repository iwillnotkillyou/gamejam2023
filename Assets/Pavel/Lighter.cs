using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightObject
{
    public Vector3 Pos { get; }
    public float Range { get; }
}

public class Lighter : MonoBehaviour, ILightObject
{
    private float buffDuration = 0;

    public float YeetRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            YeetRadius = 1f;
        }

        if (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
        }
        else if (buffDuration <= 0)
        {
            YeetRadius = 0.5f;
        }
    }

    public Vector3 Pos => transform.position;
    public float Range => YeetRadius;
}
