using UnityEngine;
using Prime31.StateKit;

public class SkyPhase4State : SKState<PhaseHandler>
{
    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetSky();
        }

        _context.music.time = GameMaster.instance.skyPhase4Start;
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.skyPhase4Start)
        {

        }
    }

    public override void end()
    {

    }
}