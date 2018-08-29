using UnityEngine;
using Prime31.StateKit;

public class SkyPhase1State : SKState<PhaseHandler>
{
    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetSky();
        }

        _context.music.time = GameMaster.instance.skyPhase1Start;

        _context.cloudFade.transform.position = Vector2.zero;
        LeanTween.moveY(_context.cloudFade, -15, 1).setEase(LeanTweenType.easeOutCubic);

        _context.skyPlayer = Object.Instantiate(_context.skyPlayerPrefab, new Vector2(-4.5f, -2), Quaternion.identity);
        _context.skyPlayer.SetTargetPosOrigin(new Vector2(-4.5f, 2));
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.skyPhase2Start)
        {
            _machine.changeState<SkyPhase2State>();
        }
    }

    public override void end()
    {

    }
}