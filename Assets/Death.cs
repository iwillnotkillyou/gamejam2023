using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public float lifetime = 1;
    [SerializeField]
    GameObject dyingBalls;
    List<GameObject> List = new List<GameObject>();
    private void Awake()
    {

        int number = Random.Range(4,10);
        for (int i = 0; i < number; i++)
        {
            GameObject ball = Instantiate(dyingBalls,gameObject.transform.position,Quaternion.identity);
            ball.gameObject.transform.localScale = new Vector2(0.5f,0.5f);
            List.Add(ball);
            float xSpawn = Random.Range(-0.25f, 0.25f);
            float ySpawn = Random.Range(-0.25f, 0.25f);
            ball.transform.position += new Vector3(xSpawn,ySpawn);
            ball.transform.GetComponent<Rigidbody2D>().velocity += new Vector2(xSpawn,ySpawn) * 2;
        }
    }
    private void Update()
    {
        lifetime -= 0.25f * Time.fixedDeltaTime; 
        foreach (var item in List)
        {
            Color tmp = item.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = lifetime;
            item.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }
        if (lifetime <= 0)
        {
            Debug.Log("died");
            foreach (var item in List) { Destroy(item); }
            Destroy(gameObject);
        }
    }
}
