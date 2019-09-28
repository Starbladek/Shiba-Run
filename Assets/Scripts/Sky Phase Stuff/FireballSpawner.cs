using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public float spawnTimerLength;
    float spawnTimer;

    public Fireball fireballPrefab;
    public List<float> altDegs;
    int altDegCount;



    void Start()
    {
        transform.name = "Fireball Spawner";
        spawnTimer = spawnTimerLength;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTimerLength)
        {
            spawnTimer = spawnTimer - spawnTimerLength;
            Fireball fireball = Instantiate(fireballPrefab);

            if (altDegs.Count > 0)
            {
                fireball.SetMainDeg(altDegs[altDegCount]);
                altDegCount++;
                if (altDegCount > altDegs.Count - 1) altDegCount = 0;
            }
        }
    }
}