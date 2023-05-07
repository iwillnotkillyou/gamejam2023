using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour
{
    float maxX = 0;
    float maxY = 0;
    bool grown = false;
    float lifeTime = 0;
    float maximumLifetime;
    private void Awake()
    {
        maximumLifetime = Random.Range(5,10);
    }
    private void Update()
    {
        this.gameObject.transform.localScale = new Vector3(1, 1, 1) * lifeTime / maximumLifetime;  
        if(lifeTime < maximumLifetime)
        {
            lifeTime += 1 * Time.deltaTime;
        }
        else
        {
            if (!grown)
            {
                grown = true;
                SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
                List<Vector2> positions = new List<Vector2>();
                //Right
                if (gameObject.transform.position.x + sr.bounds.size.x/2 < maxX) { positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x/2,0)); }
                //Left
                if (gameObject.transform.position.x - sr.bounds.size.x / 2 > 0) { positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2, 0)); }
                //Top
                if (gameObject.transform.position.y + sr.bounds.size.y / 2 < maxY) { positions.Add(gameObject.transform.position + new Vector3(0, sr.bounds.size.y / 2)); }
                //Bottom
                if (gameObject.transform.position.y - sr.bounds.size.y / 2 > 0) { positions.Add(gameObject.transform.position + new Vector3(0, -sr.bounds.size.y / 2)); }
                //TopRight
                if (gameObject.transform.position.x + sr.bounds.size.x / 2 < maxX && gameObject.transform.position.y + sr.bounds.size.y / 2 < maxY) {
                    positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x / 2, sr.bounds.size.y / 2));
                }
                //TopLeft
                if (gameObject.transform.position.y + sr.bounds.size.y / 2 < maxY && gameObject.transform.position.x - sr.bounds.size.y / 2 > 0) { 
                    positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2, sr.bounds.size.y / 2)); 
                }
                //BottomLeft
                if (gameObject.transform.position.y - sr.bounds.size.y / 2 > 0 && gameObject.transform.position.x - sr.bounds.size.x / 2 > 0) {
                    positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2, -sr.bounds.size.y / 2));
                }
                //BottomRight
                if(gameObject.transform.position.y - sr.bounds.size.y / 2 > 0 && gameObject.transform.position.x + sr.bounds.size.x / 2 < maxX)
                {
                    positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x / 2, -sr.bounds.size.y / 2));
                }
                for (int i = 0; i < 3; i++)
                {
                    int pick = Random.Range(0, positions.Count);
                    Instantiate(gameObject, positions[pick], Quaternion.identity);
                    positions.RemoveAt(pick);
                }
            }
        }
        if(Vector2.Distance(CandleInformer.CandleLocation,gameObject.transform.position) < 5)
        {
            lifeTime -= Time.deltaTime * 5;
            if(lifeTime < 4) { Destroy(gameObject); }
        }
    }
}
/*
 halfWidth = spriteRenderer.bounds.size.x/2;
 halfHeight = spriteRenderer.bounds.size.y/2;
 */
