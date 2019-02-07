using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftingShot : MonoBehaviour
{
    Vector3 homePos;
    float speed = 6;

    float mainDeg;
    float sinValue;
    float sinSpeed;



    void Start()
    {
        homePos = transform.position;
        sinSpeed = GameMaster.instance.quarterNoteLength * 2;
    }

    void Update()
    {
        homePos += (Vector3.left * speed * Time.deltaTime);

        sinValue = 2 * Mathf.Sin(mainDeg * Mathf.Deg2Rad);
        mainDeg += (360 * (1 / sinSpeed)) * Time.deltaTime;
        if (mainDeg > 360) mainDeg = mainDeg - 360;

        Vector3 finalPos = new Vector3(homePos.x + sinValue, homePos.y, homePos.z);
        transform.position = finalPos;

        if (transform.position.x <= GameMaster.instance.screenLeftEdge - 1f)
            Destroy(gameObject);
    }
}