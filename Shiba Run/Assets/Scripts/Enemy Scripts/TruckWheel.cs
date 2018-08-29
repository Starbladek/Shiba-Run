using UnityEngine;

public class TruckWheel : MonoBehaviour
{
    Vector2 velocity;
    float acceleration = 1;
    public Vector2 overrideSpawnPos;
    public Vector2 endPos;
    bool grounded = false;

    void Start()
    {
        velocity = MathEquations.CalculateLaunchVelocity(transform.position, endPos, -GameMaster.instance.gravity, 4);
    }

    void Update()
    {
        velocity.x *= acceleration;
        if (!grounded) velocity.y -= GameMaster.instance.gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);
        if (transform.position.y <= 0 && grounded == false)
        {
            grounded = true;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            velocity = new Vector2(velocity.x, 0);
            acceleration = 1.1f;
        }
        if (transform.position.x < GameMaster.instance.screenLeftEdge) Destroy(gameObject);
    }
}