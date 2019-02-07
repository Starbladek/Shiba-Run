using UnityEngine;
using Prime31.StateKit;

public class Phase5State : SKState<PhaseHandler>
{
    DriftingShotSpawner driftingShotSpawner;

    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetGround();
            _context.groundPlayer = Object.Instantiate(_context.groundPlayerPrefab, new Vector2(-3.5f, 0f), Quaternion.identity);
            _context.groundPlayer.SetTargetPosOriginitionX(-3.5f);
            _context.groundScenery.active = true;
        }

        _context.music.time = GameMaster.instance.phase5Start;
        driftingShotSpawner = Object.Instantiate(_context.driftingShotSpawnerPrefab);
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase6Start)
        {
            _machine.changeState<Phase6State>();
        }
    }

    public override void end()
    {
        Object.Destroy(driftingShotSpawner);
    }
}