using UnityEngine;

public class TruckFootball : MonoBehaviour
{
    Vector2 velocity;
    public Vector2[] landingSpots;
    public float[] flightHeights;
    int currentFlight;
    float rotationAmount;

    void Start()
    {
        rotationAmount = Random.Range(400f, 2000f);
        velocity = MathEquations.CalculateLaunchVelocity(transform.position, landingSpots[0], -GameMaster.instance.gravity, flightHeights[0]);
    }

    void Update()
    {
        velocity.y -= GameMaster.instance.gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * rotationAmount * Time.deltaTime);
        if (transform.position.y <= 0 && currentFlight < landingSpots.Length)
        {
            currentFlight++;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            velocity = MathEquations.CalculateLaunchVelocity(transform.position, landingSpots[currentFlight], -GameMaster.instance.gravity, flightHeights[currentFlight]);
            rotationAmount = Random.Range(400f, 2000f);
            if (velocity.x > 0) rotationAmount = -rotationAmount;
        }
        if (transform.position.x < GameMaster.instance.screenLeftEdge) Destroy(gameObject);
    }
}