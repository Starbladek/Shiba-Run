using UnityEngine;

public class Fireball : MonoBehaviour
{
    float mainDeg;
    float sinValue;
    float sinSpeed;
    float minYVal;
    float maxYVal;
    public float lateralSpeed;



    void Start()
    {
        //Upper value can be anywhere between 1 below the top of the screen and 5 above the bottom of the screen
        maxYVal = GameMaster.instance.screenTopEdge - Random.Range(1, (GameMaster.instance.screenTopEdge - GameMaster.instance.screenBottomEdge) - 5);
        //Lower value can be anywhere between 1 above the bottom of the screen and 1 below the upper value
        minYVal = GameMaster.instance.screenBottomEdge + Random.Range(1, (maxYVal - GameMaster.instance.screenBottomEdge) - 1);
        sinSpeed = GameMaster.instance.barLength;

        float spawnPosX = GameMaster.instance.screenRightEdge + 1;
        float spawnPosY = minYVal + ((maxYVal - minYVal) / 2) * (1 + Mathf.Sin(mainDeg * Mathf.Deg2Rad));
        transform.position = new Vector2(spawnPosX, spawnPosY);

        lateralSpeed = Random.Range(lateralSpeed - 2, lateralSpeed + 2);
    }

    void Update()
    {
        //Move X
        float xPos = transform.position.x - (lateralSpeed * Time.deltaTime);

        //Move Y
        sinValue = minYVal + ((maxYVal - minYVal) / 2f) * (1 + Mathf.Sin(mainDeg * Mathf.Deg2Rad));
        mainDeg += (360 * (1 / sinSpeed)) * Time.deltaTime;
        if (mainDeg > 360) mainDeg = mainDeg - 360;

        Vector2 prevPos = transform.position;
        transform.position = new Vector2(xPos, sinValue);

        //This setup causes twitching during lag?...
        float AngleRad = Mathf.Atan2(prevPos.y - transform.position.y, prevPos.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;
        if (angle != transform.localEulerAngles.z) transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angle);

        if (transform.position.x < GameMaster.instance.screenLeftEdge - 0.75f)
        {
            Destroy(gameObject);
        }
    }

    public void SetMainDeg(float val) { mainDeg = val; }
}