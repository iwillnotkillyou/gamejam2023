using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Grower : MonoBehaviour
{
    public float maxX = 0;
    public float maxY = 0;
    bool grown = false;
    float lifeTime = 0;
    float maximumLifetime;
    public GameObject Death;
    private void Start()
    {
        if(GetEnemies(this.gameObject.GetComponent<Collider2D>()).Count > 0)
        {
            Destroy(this.gameObject);
        }
        maximumLifetime = Random.Range(5,10);
        this.gameObject.transform.localScale = new Vector3(0.001f, 0.001f);
    }
    private void FixedUpdate()
    {
         if (Vector2.Distance(Lighter.mainLighter.gameObject.transform.position, this.gameObject.transform.position) < 0.1f)
        {
            CandleInformer.DamagePlayer(0.10f);
            Instantiate(Death, this.gameObject.transform.position, Quaternion.identity); Destroy(gameObject);
        }
        this.gameObject.transform.localScale = new Vector3(1, 1, 1) * lifeTime / maximumLifetime;  
        if(lifeTime < maximumLifetime)
        {
            lifeTime += 1 * Time.fixedDeltaTime;
        }
        else
        {
            if (!grown)
            {
                grown = true;
                SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
                List<Vector2> positions = new List<Vector2>();
                //Right
                if (gameObject.transform.position.x + sr.bounds.size.x/2 + 0.1f < maxX) { positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x / 2 + 0.1f,0)); }
                //Left
                if (gameObject.transform.position.x - sr.bounds.size.x / 2 - 0.1f > 0) { positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2 - 0.1f, 0)); }
                //Top
                if (gameObject.transform.position.y + sr.bounds.size.y / 2 + 0.1f < maxY) { positions.Add(gameObject.transform.position + new Vector3(0, sr.bounds.size.y / 2 + 0.1f)); }
                //Bottom
                if (gameObject.transform.position.y - sr.bounds.size.y / 2 - 0.1f > 0) { positions.Add(gameObject.transform.position + new Vector3(0, -sr.bounds.size.y / 2 - 0.1f)); }
                //TopRight
                if (gameObject.transform.position.x + sr.bounds.size.x / 2 + 0.1f < maxX && gameObject.transform.position.y + sr.bounds.size.y / 2 + 0.1f < maxY) {
                    positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x / 2 + 0.1f, sr.bounds.size.y / 2 + 0.1f));
                }
                //TopLeft
                if (gameObject.transform.position.y + sr.bounds.size.y + 0.1f / 2 < maxY && gameObject.transform.position.x - sr.bounds.size.y / 2 -0.1f > 0) { 
                    positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2 -0.1f, sr.bounds.size.y / 2 + 0.1f)); 
                }
                //BottomLeft
                if (gameObject.transform.position.y - sr.bounds.size.y / 2 -0.1f > 0 && gameObject.transform.position.x - sr.bounds.size.x / 2 -0.1f > 0) {
                    positions.Add(gameObject.transform.position + new Vector3(-sr.bounds.size.x / 2 -0.1f, -sr.bounds.size.y / 2 - 0.1f));
                }
                //BottomRight
                if(gameObject.transform.position.y - sr.bounds.size.y / 2 -0.1f > 0 && gameObject.transform.position.x + sr.bounds.size.x / 2 +0.1f < maxX)
                {
                    positions.Add(gameObject.transform.position + new Vector3(sr.bounds.size.x / 2 + 0.1f, -sr.bounds.size.y / 2 - 0.1f));
                }
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        int pick = Random.Range(0, positions.Count - 1);
                        if (pick < positions.Count)
                        {
                            Instantiate(gameObject, positions[pick], Quaternion.identity);
                            positions.RemoveAt(pick);
                        }
                    }
                    catch
                    {

                    }

                    }
                }
        }
        if(Vector2.Distance(CandleInformer.CandleLocation,gameObject.transform.position) < 4.5f)
        {
            lifeTime -= Time.fixedDeltaTime * 5;
            if(lifeTime < 4) { Instantiate(Death,this.gameObject.transform.position,Quaternion.identity); Destroy(gameObject); }
        }
    }
    public static List<GameObject> GetEnemies(Collider2D col)
    {
        var currentCollisions1 = GameObject
            .FindGameObjectsWithTag("Enemy")
            .Where(x => x != col.gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(currentCollisions1.Count);
        var currentCollisions = currentCollisions1
            .Where(x => x.Distance(col).distance < 0.01)
            .Select(x => x.gameObject).ToList();
        return currentCollisions;
    }
}
/*
 halfWidth = spriteRenderer.bounds.size.x/2;
 halfHeight = spriteRenderer.bounds.size.y/2;
 */
