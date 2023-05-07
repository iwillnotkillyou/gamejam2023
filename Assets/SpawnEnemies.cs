using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static GameObject mainSpawner;
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
    private void Start()
    {
        mainSpawner = this.gameObject;
        gameObject.SetActive(false);
        Table = GameObject.FindGameObjectsWithTag("Table")[0];
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
                    newBorn.gameObject.GetComponent<Grower>().Death = Death;
                } 
                else {
                    GameObject newBorn = Instantiate(demons[1], pos, Quaternion.identity);
                    newBorn.GetComponent<Enemy_movement>().speed = spawnSpeed;
                    newBorn.gameObject.GetComponent<Enemy_movement>().Death = Death;
                }
            }
        }
    }
    private void Update()
    {
        float spawnRate = 0;
        timer += Time.fixedDeltaTime * 1;
        switch (decayLevel)
        {
            case 0:
                spawnRate = speed[0];
                //speed 1/4
                //spawn 1 per 4-6 seconds
                Spawn(spawnRate,1);
                break;
            case 1:
                spawnRate = speed[1];
                //speed 2/4
                //spawn 1 per
                Spawn(spawnRate,1);
                break;
            case 2:
                spawnRate = speed[2];
                Spawn(spawnRate, 2);
                break;
            case 3:
                spawnRate = speed[3];
                Spawn(spawnRate, 2);
                break;
            case 4:
                spawnRate = speed[4];
                Spawn(spawnRate, 3);
                break;
            default:
                break;
        }
    }
}