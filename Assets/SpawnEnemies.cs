using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public GameObject Death;
    float maxX = 30;
    float maxY = 20;
    int decayLevel = 0;
    [SerializeField]
    List<GameObject> demons;
    [SerializeField]
    List<float> speed;
    public float spawnSpeed;
    GameObject Table;
    float timer = 0;
    float spawnTime = 0;
    private void Awake()
    {
        Table = GameObject.FindGameObjectsWithTag("Table")[0];
        maxX = Table.GetComponent<SpriteRenderer>().bounds.size.x;
        maxY = Table.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    public void Decay()
    {
        decayLevel++;
    }
    void Spawn(float SpawnRate,float SpawnAmount)
    {
        Vector2 playerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //find player
        Vector3 pos;
        while (Vector3.Distance(pos = new Vector2(Random.Range(0, maxX), Random.Range(0, maxY)), playerPosition) < 2) { }
        if (SpawnRate <= timer)
        {
            timer = 0;
            for (int i = 0; i < SpawnAmount; i++) {
                if(Random.Range(0,100) > 85) { GameObject newBorn = Instantiate(demons[0], pos, Quaternion.identity);
                    newBorn.gameObject.GetComponent<Grower>().speed = spawnSpeed;
                    newBorn.gameObject.GetComponent<Grower>().maxX = maxX;
                    newBorn.gameObject.GetComponent<Grower>().maxY = maxY;
                    newBorn.gameObject.GetComponent<Grower>().Death = Death;
                } 
                else {
                    GameObject newBorn = Instantiate(demons[1], pos, Quaternion.identity);
                    newBorn.GetComponent<Enemy_movement>().speed = spawnSpeed;
                    newBorn.gameObject.GetComponent<Enemy_movement>().maxX = maxX;
                    newBorn.gameObject.GetComponent<Enemy_movement>().maxY = maxY;
                    newBorn.gameObject.GetComponent<Enemy_movement>().Death = Death;
                }
            }
        }
    }
    private void Update()
    {
        float spawnSpeed = 0;
        timer += Time.deltaTime * 1;
        switch (decayLevel)
        {
            case 0:
                spawnSpeed = speed[0];
                //speed 1/4
                //spawn 1 per 4-6 seconds
                Spawn(5,1);
                break;
            case 1:
                spawnSpeed = speed[1];
                //speed 2/4
                //spawn 1 per
                Spawn(3.5f,1);
                break;
            case 2:
                spawnSpeed = speed[2];
                Spawn(3.5f, 2);
                break;
            case 3:
                spawnSpeed = speed[3];
                Spawn(3f, 2);
                break;
            case 4:
                spawnSpeed = speed[4];
                Spawn(2, 3);
                break;
            default:
                break;
        }
    }
}