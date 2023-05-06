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
    float spawnSpeed = 0;
    [SerializeField]
    GameObject basicDemon;
    [SerializeField]
    List<float> speed;
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
            for(int i = 0; i < SpawnAmount; i++) {
                Debug.Log("spawning");
                Instantiate(basicDemon,pos,Quaternion.identity);
            }
        }
    }
    private void Update()
    {
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
                Spawn(3f,1);
                break;
            case 2:
                spawnSpeed = speed[2];
                Spawn(3f, 2);
                break;
            case 3:
                spawnSpeed = speed[3];
                Spawn(2.5f, 2);
                break;
            case 4:
                spawnSpeed = speed[4];
                Spawn(2f, 3);
                break;
            default:
                break;
        }
    }
}