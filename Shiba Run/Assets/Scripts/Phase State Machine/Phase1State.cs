using UnityEngine;
using Prime31.StateKit;

public class Phase1State : SKState<PhaseHandler>
{
    public override void begin()
    {
        if (_context.currentlyResetting)
        {
            _context.currentlyResetting = false;
            _context.ResetGround();
            _context.groundScenery.active = false;
            _context.groundScenery.streetSegments[(_context.groundScenery.numOfSegments / 2) - 1].GetComponent<SpriteRenderer>().sprite = _context.groundScenery.houseSpriteUnbroken;
        }

        _context.music.time = GameMaster.instance.phase1Start;
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase2Start)
        {
            
        }
    }

    public override void end()
    {
        
    }
}