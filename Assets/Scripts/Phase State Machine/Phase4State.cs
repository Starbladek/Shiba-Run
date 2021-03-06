﻿using UnityEngine;
using Prime31.StateKit;

public class Phase4State : SKState<PhaseHandler>
{
    LavaShotSpawner lavaShotSpawner;

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

        _context.music.time = GameMaster.instance.phase4Start;
        lavaShotSpawner = Object.Instantiate(_context.lavaShotSpawnerPrefab);
        lavaShotSpawner.player = _context.groundPlayer;
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase5Start)
        {
            _machine.changeState<Phase5State>();
        }
    }

    public override void end()
    {
        Object.Destroy(lavaShotSpawner);
    }
}