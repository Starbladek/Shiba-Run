using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class PhaseHandler : MonoBehaviour
{
    SKStateMachine<PhaseHandler> machine;

    [HideInInspector]
    public bool currentlyResetting;

    [HideInInspector]
    public GroundPlayer groundPlayer;
    [HideInInspector]
    public SkyPlayer skyPlayer;
    GliderTransition glider;

    [HideInInspector]
    public SceneryScrolling groundScenery;

    public CameraController cameraController;
    public GameObject cloudFade;
    public AudioSource music;
    public GroundPlayer groundPlayerPrefab;
    public SceneryScrolling groundSceneryPrefab;

    public SimpleObject housePlankPrefab;
    public Truck truckPrefab;
    public LavaShotSpawner lavaShotSpawnerPrefab;
    public MeteorSpawner meteorSpawnerPrefab;
    public GliderTransition gliderTransitionPrefab;

    public SkyPlayer skyPlayerPrefab;
    public FireballSpawner fireballSpawnerPrefab;

    public Sprite[] debrisSprites;



    void Start()
    {
        machine = new SKStateMachine<PhaseHandler>(this, new Phase1State());

        machine.addState(new Phase2State());
        machine.addState(new Phase3State());
        machine.addState(new Phase4State());
        machine.addState(new Phase5State());
        machine.addState(new Phase6State());
        machine.addState(new Phase7State());
        machine.addState(new Phase8State());
        machine.addState(new Phase9State());

        machine.addState(new SkyPhase1State());
        machine.addState(new SkyPhase2State());
        machine.addState(new SkyPhase3State());
        machine.addState(new SkyPhase4State());

        music.Play();
        groundScenery = Instantiate(groundSceneryPrefab);
        GameMaster.instance.phase7Start = GameMaster.instance.phase7Start - meteorSpawnerPrefab.meteorEntity.fallTimerLength;
        GameMaster.instance.phase8Start = GameMaster.instance.phase8Start - meteorSpawnerPrefab.meteorEntity.fallTimerLength;
    }

    void Update()
    {
        machine.update(Time.deltaTime);
        music.pitch = Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Alpha1) && machine.currentState.GetType() != typeof(Phase1State))
        {
            currentlyResetting = true;
            machine.changeState<Phase1State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && machine.currentState.GetType() != typeof(Phase2State))
        {
            currentlyResetting = true;
            machine.changeState<Phase2State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && machine.currentState.GetType() != typeof(Phase3State))
        {
            currentlyResetting = true;
            machine.changeState<Phase3State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && machine.currentState.GetType() != typeof(Phase4State))
        {
            currentlyResetting = true;
            machine.changeState<Phase4State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && machine.currentState.GetType() != typeof(Phase5State))
        {
            currentlyResetting = true;
            machine.changeState<Phase5State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && machine.currentState.GetType() != typeof(Phase6State))
        {
            currentlyResetting = true;
            machine.changeState<Phase6State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && machine.currentState.GetType() != typeof(Phase7State))
        {
            currentlyResetting = true;
            machine.changeState<Phase7State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && machine.currentState.GetType() != typeof(Phase8State))
        {
            currentlyResetting = true;
            machine.changeState<Phase8State>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && machine.currentState.GetType() != typeof(Phase9State))
        {
            currentlyResetting = true;
            machine.changeState<Phase9State>();
        }

        if (Input.GetKeyDown(KeyCode.E) && machine.currentState.GetType() != typeof(SkyPhase1State))
        {
            currentlyResetting = true;
            machine.changeState<SkyPhase1State>();
        }
        if (Input.GetKeyDown(KeyCode.R) && machine.currentState.GetType() != typeof(SkyPhase2State))
        {
            currentlyResetting = true;
            machine.changeState<SkyPhase2State>();
        }
        if (Input.GetKeyDown(KeyCode.T) && machine.currentState.GetType() != typeof(SkyPhase3State))
        {
            currentlyResetting = true;
            machine.changeState<SkyPhase3State>();
        }
    }

    public void ResetGround()
    {
        ClearScreen();

        groundScenery.gameObject.SetActive(true);
        groundScenery.reset();
        if (glider != null) glider.StopAllCoroutines();
        cloudFade.transform.position = new Vector2(0, 19);
        LeanTween.cancel(cloudFade);
        LeanTween.cancel(cameraController.gameObject);
        cameraController.transform.position = cameraController.originPos;
    }

    public void ResetSky()
    {
        ClearScreen();

        groundScenery.gameObject.SetActive(false);
        cloudFade.transform.position = new Vector2(0, -15);
        LeanTween.cancel(cloudFade);
        if (machine.currentState.GetType() != typeof(SkyPhase1State) || machine.currentState.GetType() != typeof(SkyPhase2State))
            cameraController.transform.position = cameraController.originPos;
        else
            cameraController.transform.position = cameraController.originPos * 1.5f;
    }

    void ClearScreen()
    {
        GameMaster.instance.gameOverTransparentCover.color = new Color(0f, 0f, 0f, 0f);
        GameMaster.instance.timeScale = 1f;

        GameObject[] playerEntities = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] loseEntitySpawners = GameObject.FindGameObjectsWithTag("LoseEntitySpawner");
        GameObject[] loseEntities = GameObject.FindGameObjectsWithTag("LoseEntity");
        GameObject[] junkEntities = GameObject.FindGameObjectsWithTag("Junk");
        foreach (GameObject player in playerEntities) Destroy(player);
        foreach (GameObject loseEntity in loseEntities) Destroy(loseEntity);
        foreach (GameObject loseEntitySpawner in loseEntitySpawners) Destroy(loseEntitySpawner);
        foreach (GameObject junkEntity in junkEntities) Destroy(junkEntity);

        LeanTween.cancel(gameObject);
        LeanTween.cancel(GameMaster.instance.gameObject);

        StopAllCoroutines();
        GameMaster.instance.StopAllCoroutines();
    }

    public MeteorSpawner CreateMeteorSpawner(float meteorStartPos, float meteorXTranslateAmount)
    {
        MeteorSpawner meteorSpawner = Instantiate(meteorSpawnerPrefab);
        meteorSpawner.hitPosX = meteorStartPos;
        meteorSpawner.xTranslationOnNextMeteor = meteorXTranslateAmount;
        return meteorSpawner;
    }

    IEnumerator CliffRoutine()
    {
        yield return new WaitForSeconds(1);
        groundScenery.StartCliffMode();
        groundPlayer.SetCliffJump();
        yield return new WaitForSeconds(2);
        glider = Instantiate(gliderTransitionPrefab, Vector2.zero, Quaternion.identity);
    }
}