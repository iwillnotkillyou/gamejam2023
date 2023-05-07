using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    float maxX = 0;
    [SerializeField]
    float maxY = 0;
    int decayLevel = 0;
    [SerializeField]
    GameObject basicDemon;
    [SerializeField]
    List<float> speed = new List<float>(){5,3,3,2.5f,2};
    float timer = 0;
    float spawnTime = 0;
    public void Decay()
    {
        decayLevel++;
    }
    void Spawn(float SpawnRate,float SpawnAmount)
    {
        Vector2 playerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //find player
        Vector3 pos;
        while (Vector3.Distance(pos = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY)), playerPosition) < 2) { }
        if (SpawnRate <= timer)
        {
            timer = 0;
            for (int i = 0; i < SpawnAmount; i++) {
                GameObject newBorn = Instantiate(basicDemon, pos, Quaternion.identity);
                //newBorn.GetComponent<Enemy_movement>().speed = spawnSpeed;
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
                Spawn(spawnSpeed,1);
                break;
            case 1:
                spawnSpeed = speed[1];
                //speed 2/4
                //spawn 1 per
                Spawn(spawnSpeed,1);
                break;
            case 2:
                spawnSpeed = speed[2];
                Spawn(spawnSpeed, 2);
                break;
            case 3:
                spawnSpeed = speed[3];
                Spawn(spawnSpeed, 2);
                break;
            case 4:
                spawnSpeed = speed[4];
                Spawn(spawnSpeed, 3);
                break;
            default:
                break;
        }
    }
}