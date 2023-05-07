using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CandleInformer : MonoBehaviour
{
    public float MaxLife = 100;
    public static float playerLife = 100;

    public static void DamagePlayer(float dmg)
    {
        playerLife -= dmg;
    }
    public static Vector2 CandleLocation;
    private void Awake()
    {
        CandleLocation = transform.position;
        playerLife = MaxLife;
    }
    void Update()
    {
        CandleLocation = transform.position;
        playerLife -= 1 * Time.fixedDeltaTime;
        this.gameObject.transform.localScale = new Vector2(playerLife/MaxLife*0.2f,playerLife/MaxLife*0.2f);
    }
}
