using UnityEngine;

public class Baby : MonoBehaviour
{
    Vector2 velocity;
    public Vector2 endPos;
    public float babyApex;

    void Start()
    {
        velocity = MathEquations.CalculateLaunchVelocity(transform.position, endPos, -4f, babyApex);
    }

    void Update()
    {
        velocity.y -= 4f * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * 25f * Time.deltaTime);
        if (transform.position.x < GameMaster.instance.screenLeftEdge) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<GroundPlayer>().SetGotBaby(false);
            Destroy(gameObject);
        }
    }
}