using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//GameMaster is an example of SINGLETON design, where we have one instance of an object (and we know there will only
//ever be one instance of it) that all other scripts can access

//TODO:
//Important
//  -Brainstorm obstacles that could be in the flying section
//      *Premade normal fireballs that travel in a sine wave, but what is their spawn pattern?
//      *Partially randomly spawned green fireballs that have a slight homing effect?
//      *Chunks of debris that shoot out of the bottom of the screen on every drum snap?
//  -Checkpoint/reset system
//
//Polish
//  -Potentially add in static variable that keeps track of every eighth/quarter/bar/phase note, and pulses out a signal each time
//  -Improve house debris spawning
//  -Put lava at the end of the ground phase
//  -Firey earth in the background of the sky phase that subtly rotates

public class GameMaster : MonoBehaviour
{
    //GLOBAL VARIABLES
    public static GameMaster instance;

    //public static float bpm = 176f; //Dunno if this is correct, so I'm not using it
    [HideInInspector]
    public float eighthNoteLength = 0.3342f;

    [HideInInspector]
    public float quarterNoteLength;  //= 2 eighths

    [HideInInspector]
    public float barLength;          //= 4 quarters

    [HideInInspector]
    public float phaseLength;        //= 4 bars

    [HideInInspector]
    public float screenTopEdge;
    [HideInInspector]
    public float screenBottomEdge;
    [HideInInspector]
    public float screenLeftEdge;
    [HideInInspector]
    public float screenRightEdge;

    public float phase1Start = 0.25f;
    public float phase2Start = 62.00f;
    public float phase3Start = 64.25f;
    public float phase4Start = 74.85f;
    public float phase5Start = 85.45f;
    public float phase6Start = 96.15f;
    public float phase7Start = 106.83f;
    public float phase8Start = 117.50f;
    public float phase9Start = 122.05f;

    public float skyPhase1Start = 128.00f;
    public float skyPhase2Start = 139.25f;
    public float skyPhase3Start = 149.50f;
    public float skyPhase4Start = 999.99f;

    public Canvas gameOverCanvas;
    public Image gameOverTransparentCover;

    public float gravity = 20;
    public float timeScale = 1;



    void Start()
    {
        instance = this;

        quarterNoteLength = eighthNoteLength * 2;
        barLength = quarterNoteLength * 4;
        phaseLength = barLength * 4;

        screenTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -(Camera.main.transform.position.z))).y;
        screenBottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).y;
        screenLeftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).x;
        screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -(Camera.main.transform.position.z))).x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (timeScale == 1f)
            {
                LeanTween.value(gameObject, 1f, 0.5f, 0.5f).setOnUpdate((float value) =>
                {
                    timeScale = value;
                }).setEase(LeanTweenType.easeOutCubic).setUseEstimatedTime(true);
            }
            else if (timeScale == 0.5f)
            {
                LeanTween.value(gameObject, 0.5f, 1f, 0.5f).setOnUpdate((float value) =>
                {
                    timeScale = value;
                }).setEase(LeanTweenType.easeInCubic).setUseEstimatedTime(true);
            }
        }
        Time.timeScale = timeScale;

        //Constantly update the screen bounds (could be performance taxing?)
        screenTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -(Camera.main.transform.position.z))).y;
        screenBottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).y;
        screenLeftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).x;
        screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -(Camera.main.transform.position.z))).x;
    }

    public void DeathTransition()
    {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        //Lightly darken the screen over the course of two seconds
        LeanTween.value(gameObject, 0f, 0.5f, 2f).setOnUpdate((float value) =>
        {
            gameOverTransparentCover.color = new Color(0f, 0f, 0f, value);
        }).setEase(LeanTweenType.easeOutCubic).setUseEstimatedTime(true);

        //Slow the timeScale down to 0 over the course of two seconds
        LeanTween.value(gameObject, 1f, 0f, 2f).setOnUpdate((float value) =>
        {
            timeScale = value;
        }).setEase(LeanTweenType.easeOutCubic).setUseEstimatedTime(true);

        //Wait two seconds
        yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(2));

        //Play click sound
        //generalAudioManager.PlayOneShot(retrySound);

        //Intensely brighten screen over the course of 0.1s
        LeanTween.value(gameObject, new Color(0f, 0f, 0f, 0.5f), new Color(1f, 1f, 1f, 0.9f), 0.1f).setOnUpdate((Color value) =>
        {
            gameOverTransparentCover.color = value;
        }).setUseEstimatedTime(true);

        //Wait 0.1s
        yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.1f));

        //Create text object
        GameObject textObject = new GameObject("Retry Text");
        textObject.transform.SetParent(gameOverCanvas.transform);
        textObject.transform.position = gameOverCanvas.transform.position;
        textObject.tag = "Junk";

        //Add text to that text object
        Text retryText = textObject.AddComponent<Text>();
        retryText.font = Resources.Load<Font>("Fonts/Eras Demi ITC");
        retryText.fontSize = 80;
        retryText.alignment = TextAnchor.MiddleCenter;
        retryText.horizontalOverflow = HorizontalWrapMode.Overflow;
        retryText.verticalOverflow = VerticalWrapMode.Overflow;
        retryText.GetComponent<Text>().text = "Retry?";

        //Lessen the brightness over the course of 0.1s
        LeanTween.value(gameObject, new Color(1f, 1f, 1f, 0.9f), new Color(1f, 1f, 1f, 0.3f), 0.1f).setOnUpdate((Color value) =>
        {
            gameOverTransparentCover.color = value;
        }).setUseEstimatedTime(true);

        //Wait 0.1s
        yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.1f));
    }
}