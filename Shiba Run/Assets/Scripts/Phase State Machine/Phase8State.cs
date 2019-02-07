using UnityEngine;
using Prime31.StateKit;

public class Phase8State : SKState<PhaseHandler>
{
    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetGround();
            _context.groundPlayer = Object.Instantiate(_context.groundPlayerPrefab, new Vector2(-3.5f, 0f), Quaternion.identity);
            _context.groundPlayer.SetTargetPosOriginitionX(-3.5f);
            _context.groundScenery.active = true;
            _context.CreateMeteorSpawner(GameMaster.instance.screenRightEdge, -2f);
            Object.Instantiate(_context.roofBirdSpawnerPrefab);
        }

        _context.music.time = GameMaster.instance.phase8Start;
        LeanTween.delayedCall(_context.gameObject, GameMaster.instance.eighthNoteLength, () =>
        {
            _context.CreateMeteorSpawner(GameMaster.instance.screenRightEdge, -2f);
        });
        LeanTween.delayedCall(_context.gameObject, 4f, () =>
        {
            GameObject[] loseEntitySpawners = GameObject.FindGameObjectsWithTag("LoseEntitySpawner");
            foreach (GameObject loseEntitySpawner in loseEntitySpawners) Object.Destroy(loseEntitySpawner);
        });
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase9Start)
        {
            _machine.changeState<Phase9State>();
        }
    }

    public override void end()
    {
        
    }
}