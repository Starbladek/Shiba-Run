using UnityEngine;

public class SimpleObject : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float gravity = 0;
    public bool canHitGround = false;
    public float bounceFactor = 1;
    public float xAcceleration = 0;
    public enum DeleteType { None, OffScreenTop, OffScreenBottom, OffScreenLeft, OffScreenRight, OffScreenAny };
    public DeleteType deleteType = DeleteType.None;

    public void LaunchObject(Vector2 _endPos, float _maxHeight)
    {
        velocity = MathEquations.CalculateLaunchVelocity(transform.position, _endPos, -gravity, _maxHeight);
    }

    public delegate void BounceAction(GameObject objectToAccel);
    public static event BounceAction OnBounce;

    void Update()
    {
        velocity.x += xAcceleration * Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);

        if (canHitGround)
        {
            if (transform.position.y < 0)
            {
                transform.position = new Vector2(transform.position.x, -transform.position.y);
                velocity.y = -velocity.y * bounceFactor;
                if (OnBounce != null) OnBounce(gameObject); //This triggers for every object subscribed to the event on screen for some reason?
                                                            //For example, if a single object bounces while it's the only object active, it runs the OnBounce
                                                            //event once. If a single object bounces while there are five other objects active, it runs the
                                                            //OnBounce event six times, resulting in the velocity of the object being multiplied by 12
                                                            //rather than 2.
            }
        }

        switch (deleteType)
        {
            case DeleteType.OffScreenTop:
                if (transform.position.y > GameMaster.instance.screenTopEdge)
                    Destroy(gameObject);
                break;
            case DeleteType.OffScreenBottom:
                if (transform.position.y < GameMaster.instance.screenBottomEdge)
                    Destroy(gameObject);
                break;
            case DeleteType.OffScreenLeft:
                if (transform.position.x < GameMaster.instance.screenLeftEdge)
                    Destroy(gameObject);
                break;
            case DeleteType.OffScreenRight:
                if (transform.position.x > GameMaster.instance.screenRightEdge)
                    Destroy(gameObject);
                break;
            case DeleteType.OffScreenAny:
                if (transform.position.y > GameMaster.instance.screenTopEdge ||
                    transform.position.y < GameMaster.instance.screenBottomEdge ||
                    transform.position.x < GameMaster.instance.screenLeftEdge ||
                    transform.position.x > GameMaster.instance.screenRightEdge)
                    Destroy(gameObject);
                break;
        }
    }
}