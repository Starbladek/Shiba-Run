using UnityEngine;
using Prime31.StateKit;

public class Phase7State : SKState<PhaseHandler>
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

        _context.music.time = GameMaster.instance.phase7Start;
        _context.CreateMeteorSpawner(GameMaster.instance.screenRightEdge, -2f);
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase8Start)
        {
            _machine.changeState<Phase8State>();
        }
    }

    public override void end()
    {
        
    }
}