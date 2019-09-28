using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofBirdSpawner : MonoBehaviour
{
    float timer;

    public GameObject roofBirdPrefab;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += GameMaster.instance.quarterNoteLength;
            Instantiate(roofBirdPrefab);
        }
    }
}