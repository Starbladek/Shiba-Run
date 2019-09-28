using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofBird : MonoBehaviour
{
    float speed;
    float yPos;

    float mainDeg;
    float sinValue;
    float sinSpeed;

    void Start()
    {
        speed = 5;
        yPos = transform.position.y;
        sinSpeed = GameMaster.instance.quarterNoteLength * 2;
    }

    void Update()
    {
        sinValue = 0.25f * Mathf.Sin(mainDeg * Mathf.Deg2Rad);
        mainDeg += (360 * (1 / sinSpeed)) * Time.deltaTime;
        if (mainDeg > 360) mainDeg = mainDeg - 360;

        Vector2 prevPos = transform.position;
        transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), yPos + sinValue);

        float AngleRad = Mathf.Atan2(prevPos.y - transform.position.y, prevPos.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;
        if (angle != transform.localEulerAngles.z) transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angle);

        if (transform.position.x < GameMaster.instance.screenLeftEdge - 0.75f)
            Destroy(gameObject);
    }
}