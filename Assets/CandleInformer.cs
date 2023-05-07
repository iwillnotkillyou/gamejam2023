using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CandleInformer : MonoBehaviour
{
    public static float MaxLife = 1200;
    public static float playerLife = 100;
    [SerializeField]
    List<GameObject> List;

    public static void DamagePlayer(float dmg)
    {
        if (playerLife > MaxLife * 0.1f)
        {
            playerLife -= MaxLife * dmg;
        }
        else
        {
            PlayerCharacter.Die();
        }
    }
    public static Vector2 CandleLocation;
    private void Start()
    {
        CandleLocation = transform.position;
        playerLife = MaxLife;
    }
    void Update()
    {
        CandleLocation = transform.position;
        if (playerLife > MaxLife * 0.1f)
        {
            playerLife -= 1 * Time.fixedDeltaTime;
            this.gameObject.transform.localScale = new Vector2(playerLife / MaxLife * 0.2f, playerLife / MaxLife * 0.2f);
        }
        else
        {
            foreach (var item in List) { item.SetActive(false); }
        }
    }
}
