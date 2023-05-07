using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CandleInformer : MonoBehaviour
{
    public static Vector2 CandleLocation;
    private void Awake()
    {
        CandleLocation = transform.position;
    }
    void Update()
    {
        CandleLocation = transform.position;
    }
}
