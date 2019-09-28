using UnityEngine;
using Prime31.StateKit;

public class Phase3State : SKState<PhaseHandler>
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

        _context.music.time = GameMaster.instance.phase3Start;

        _context.groundPlayer = Object.Instantiate(_context.groundPlayerPrefab, new Vector2(-5.0f, 1.5f), Quaternion.identity);
        _context.groundPlayer.SetVelocity(MathEquations.CalculateLaunchVelocity(_context.groundPlayer.transform.position, Vector2.zero, _context.groundPlayer.gravity, 1f));
        _context.groundPlayer.SetMovementType(GroundPlayer.MovementType.Intro);

        _context.groundScenery.active = true;
        _context.groundScenery.streetSegments[(_context.groundScenery.numOfSegments / 2) - 1].GetComponent<SpriteRenderer>().sprite = _context.groundScenery.houseSpriteBroken;
        for (int i = 0; i < 10; i++)
        {
            SimpleObject temp = Object.Instantiate(_context.housePlankPrefab);
            temp.transform.position = new Vector2(-5f.AddRandVal(-0.5f, 0.5f), 1f.AddRandVal(-1, 1));
            temp.GetComponent<SpriteRenderer>().sprite = _context.debrisSprites[Random.Range(0, 4)];
            temp.GetComponent<SimpleObject>().velocity += new Vector2(Random.Range(0f, 6f), Random.Range(0f, 10f));
            if (Random.Range(0, 2) == 1) temp.GetComponent<SpriteRenderer>().sortingLayerName = "In front of player";
        }

        LeanTween.delayedCall(_context.gameObject, 2, () =>
        {
            Object.Instantiate(_context.truckPrefab);
        });
    }

    public override void update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _context.currentlyResetting = true;
            begin();
        }

        if (_context.music.time >= GameMaster.instance.phase4Start)
        {
            _machine.changeState<Phase4State>();
        }
    }

    public override void end()
    {

    }
}