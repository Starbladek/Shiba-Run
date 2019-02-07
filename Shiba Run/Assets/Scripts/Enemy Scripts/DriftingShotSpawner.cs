using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftingShotSpawner : MonoBehaviour
{
    float timer;

    public GameObject driftingShotPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += GameMaster.instance.quarterNoteLength;
            Instantiate(driftingShotPrefab);
        }
    }
}