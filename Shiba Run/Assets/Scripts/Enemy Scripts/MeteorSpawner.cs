using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    float spawnTimerLength;
    float spawnTimer;
    [HideInInspector] public float hitPosX;
    public float xTranslationOnNextMeteor;
    public bool randomSpawn;
    public Meteor meteorEntity;



    void Start()
    {
        transform.name = "Meteor Spawner";
        spawnTimerLength = GameMaster.instance.quarterNoteLength;
        spawnTimer = spawnTimerLength;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTimerLength)
        {
            //Spawn the meteor and change the spawnPosX for the next one
            spawnTimer -= spawnTimerLength;
            Meteor meteor = Instantiate(meteorEntity, new Vector2(hitPosX + Random.Range(-3f, 3f), GameMaster.instance.screenTopEdge), Quaternion.identity);
            meteor.name = "Meteor";
            meteor.hitPosX = hitPosX;

            hitPosX += xTranslationOnNextMeteor;
            if (hitPosX < GameMaster.instance.screenLeftEdge)
            {
                xTranslationOnNextMeteor = -xTranslationOnNextMeteor;
                hitPosX = GameMaster.instance.screenLeftEdge;
            }
            if (hitPosX > GameMaster.instance.screenRightEdge)
            {
                xTranslationOnNextMeteor = -xTranslationOnNextMeteor;
                hitPosX = GameMaster.instance.screenRightEdge;
            }

            if (randomSpawn)
            {
                hitPosX = Random.Range(GameMaster.instance.screenLeftEdge, GameMaster.instance.screenRightEdge);
            }
        }
    }
}