using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    float timer;

    public GameObject birdPrefab;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += GameMaster.instance.quarterNoteLength * 2;
            Instantiate(birdPrefab);
            Instantiate(birdPrefab);
        }
    }
}