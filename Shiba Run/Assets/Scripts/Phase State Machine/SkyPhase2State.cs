using UnityEngine;
using Prime31.StateKit;

public class SkyPhase2State : SKState<PhaseHandler>
{
    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetSky();
            _context.skyPlayer = Object.Instantiate(_context.skyPlayerPrefab, new Vector2(-4.5f, 2), Quaternion.identity);
            _context.skyPlayer.SetTargetPosOrigin(_context.skyPlayer.transform.position);
        }

        _context.music.time = GameMaster.instance.skyPhase2Start;
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.skyPhase3Start)
        {
            _machine.changeState<SkyPhase3State>();
        }
    }

    public override void end()
    {

    }
}