using UnityEngine;
using Prime31.StateKit;

public class SkyPhase3State : SKState<PhaseHandler>
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

        _context.music.time = GameMaster.instance.skyPhase3Start;

        FireballSpawner fireballSpawner = Object.Instantiate(_context.fireballSpawnerPrefab);
        fireballSpawner.spawnTimerLength = GameMaster.instance.quarterNoteLength;
        fireballSpawner.altDegs.Add(0);
        fireballSpawner.altDegs.Add(270);
        fireballSpawner.altDegs.Add(180);
        fireballSpawner.altDegs.Add(90);

        LeanTween.moveZ(_context.cameraController.gameObject, _context.cameraController.originPos.z * 1.5f, 1).setEase(LeanTweenType.easeOutCirc);
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.T))
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