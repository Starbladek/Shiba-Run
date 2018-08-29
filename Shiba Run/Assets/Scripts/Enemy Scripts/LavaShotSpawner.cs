using UnityEngine;
using System.Collections;

public class LavaShotSpawner : MonoBehaviour
{
    public SimpleObject lavaShot;
    [HideInInspector]
    public GroundPlayer player;

    float timer;
    float[] stepTimes;
    int stepNum;

    void Start()
    {
        stepTimes = new float[5];
        stepTimes[0] = GameMaster.instance.eighthNoteLength;
        stepTimes[1] = GameMaster.instance.eighthNoteLength;
        stepTimes[2] = GameMaster.instance.quarterNoteLength;
        stepTimes[3] = GameMaster.instance.quarterNoteLength;
        stepTimes[4] = GameMaster.instance.quarterNoteLength;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += stepTimes[stepNum];
            stepNum++;
            if (stepNum >= stepTimes.Length) stepNum = 0;
            Instantiate(lavaShot, new Vector2(GameMaster.instance.screenRightEdge + 0.5f, 1f.AddRandVal(0, 2)), Quaternion.identity).LaunchObject(_endPos: new Vector2(player.transform.position.x, 0), _maxHeight: 2f.AddRandVal(0, 2));
        }
    }
}