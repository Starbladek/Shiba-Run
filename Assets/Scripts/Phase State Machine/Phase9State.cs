using UnityEngine;
using Prime31.StateKit;

public class Phase9State : SKState<PhaseHandler>
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
        }

        _context.music.time = GameMaster.instance.phase9Start;
        _context.StartCoroutine("CliffRoutine");
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.skyPhase1Start)
        {
            _machine.changeState<SkyPhase1State>();
        }
    }

    public override void end()
    {

    }
}