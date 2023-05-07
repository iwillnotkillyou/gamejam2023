using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static GameObject mainSpawner;

    [SerializeField]
    float maxX = 30;
    [SerializeField]
    float maxY = 20;
    int decayLevel = 0;
    [SerializeField]
    List<GameObject> demons;
    [SerializeField]
    List<float> speed;
    public float spawnSpeed;
    float timer = 0;
    float spawnTime = 0;
    public void Decay()
    {
        decayLevel++;
    }

    public void Start()
    {
        mainSpawner = gameObject;
        gameObject.SetActive(false);
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
                if(Random.Range(0,100) > 85) { GameObject newBorn = Instantiate(demons[0], pos, Quaternion.identity); } 
                else {
                    GameObject newBorn = Instantiate(demons[1], pos, Quaternion.identity);
                    newBorn.GetComponent<Enemy_movement>().speed = spawnSpeed;
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