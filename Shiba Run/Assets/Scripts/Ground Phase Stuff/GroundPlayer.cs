using UnityEngine;

public class GroundPlayer : MonoBehaviour
{
    public float speed;
    public float jumpPower;

    Vector2 velocity;
    public float gravity;
    float targetPosOriginitionX;

    bool grounded;
    bool hasDoubleJumped;
    bool gotBaby = false;
    public enum MovementType { Intro, Normal, HangGlideTransition, Death };
    MovementType movementType = MovementType.Normal;

    public Sprite shadowSprite;
    GameObject shadow;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        shadow = new GameObject("Shiba Shadow", typeof(SpriteRenderer));
        shadow.tag = "Junk";
        shadow.GetComponent<SpriteRenderer>().sprite = shadowSprite;
        UpdateShibaShadowProperties();
    }

    void Update()
    {
        UpdateShibaShadowProperties();

        switch (movementType)
        {
            //Movement for the intro
            case MovementType.Intro:
                velocity.y += gravity * Time.deltaTime;
                transform.Translate(velocity * Time.deltaTime, Space.World);
                HandleGroundCollision();
                if (grounded == true) movementType = MovementType.Normal;
                break;

            //Movement for :l
            case MovementType.Normal:
                velocity.y += gravity * Time.deltaTime;
                //Jump
                if (Input.GetKeyDown("space") && grounded == true)
                {
                    velocity.y = jumpPower;
                    anim.Play("Shiba Jump");
                }
                //Double jump
                if (Input.GetKeyDown("space") && grounded == false && hasDoubleJumped == false)
                {
                    velocity.y = jumpPower;
                    hasDoubleJumped = true;
                    if (Random.Range(1, 251) == 1) LeanTween.rotateAround(gameObject, Vector3.forward, -1800f, 0.75f);
                    else LeanTween.rotateAround(gameObject, Vector3.forward, -360f, 0.75f);
                    anim.Play("Shiba Double Jump");
                }

                float directionalInputX = Input.GetAxisRaw("Horizontal");
                velocity.x = (directionalInputX * speed) * Time.deltaTime;
                targetPosOriginitionX = targetPosOriginitionX + ((directionalInputX * speed) * Time.deltaTime);
                if (directionalInputX == 0) targetPosOriginitionX -= 2f * Time.deltaTime; //Very gently move the player backwards if they aren't pressing anything

                //If the player is off the side of the screen, set them back on it
                if (targetPosOriginitionX < GameMaster.instance.screenLeftEdge + 1.25f)
                    targetPosOriginitionX = GameMaster.instance.screenLeftEdge + 1.25f;
                else if (targetPosOriginitionX > GameMaster.instance.screenRightEdge - 1.25f)
                    targetPosOriginitionX = GameMaster.instance.screenRightEdge - 1.25f;

                //Apply the position changes to the player
                //We can't use Mathf.Lerp with transform.Translate, so we apply the changes directly to the position
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, targetPosOriginitionX, 0.1f), transform.position.y + (velocity.y * Time.deltaTime));
                HandleGroundCollision();
                break;

            //Movement for the cliff jump (This is very similar to the code for normal movement, I wonder if theres a way to simplify the two a bit so we don't need this almost needless change)
            case MovementType.HangGlideTransition:
                velocity.y += gravity * Time.deltaTime;

                float directionalInputX2 = Input.GetAxisRaw("Horizontal");
                velocity.x = (directionalInputX2 * speed) * Time.deltaTime;
                targetPosOriginitionX = targetPosOriginitionX + ((directionalInputX2 * speed) * Time.deltaTime);

                //If the player is off the side of the screen, set them back on it
                if (targetPosOriginitionX < GameMaster.instance.screenLeftEdge + 1.25f)
                    targetPosOriginitionX = GameMaster.instance.screenLeftEdge + 1.25f;
                else if (targetPosOriginitionX > GameMaster.instance.screenRightEdge - 1.25f)
                    targetPosOriginitionX = GameMaster.instance.screenRightEdge - 1.25f;

                transform.position = new Vector2(Mathf.Lerp(transform.position.x, targetPosOriginitionX, 0.1f), transform.position.y + (velocity.y * Time.deltaTime));

                if (transform.position.y <= GameMaster.instance.screenBottomEdge)
                {
                    Die();
                    GameMaster.instance.DeathTransition();
                }
                break;

            //Movement for death
            case MovementType.Death:
                velocity.y += gravity * Time.deltaTime;
                transform.Translate(velocity * Time.deltaTime, Space.World);
                break;
        }
    }

    void UpdateShibaShadowProperties()
    {
        shadow.transform.position = new Vector2(transform.position.x, -0.7f);
        float shadowScale = Mathf.Clamp(1 / ((transform.position.y * 0.5f) + 1f), 0.05f, 0.8f) * 0.4f;  //im sorry
        shadow.transform.localScale = new Vector2(shadowScale, shadowScale);
        shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Clamp(1 / ((transform.position.y * 2f) + 1f), 0.05f, 0.6f));
    }

    void HandleGroundCollision()
    {
        //If the player is below the floor, set them to the floor
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            velocity.y = 0;
            grounded = true;
            hasDoubleJumped = false;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shiba Run")) anim.Play("Shiba Run");
        }
        else
        {
            grounded = false;
        }
    }

    void Die()
    {
        anim.Play("Shiba Hit");
        velocity = new Vector2(0, 10);
        movementType = MovementType.Death;
        GetComponentInChildren<BoxCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LoseEntity")
        {
            Die();
            GameMaster.instance.DeathTransition();
        }
    }

    //Setters and getters
    public MovementType GetMovementType() { return movementType; }
    public void SetMovementType(MovementType newMovementType) { movementType = newMovementType; }

    public bool GetGotBaby() { return gotBaby; }
    public void SetGotBaby(bool newBool) { gotBaby = true; }

    public void SetVelocity(Vector2 newVelocity) { velocity = newVelocity; }

    public void SetTargetPosOriginitionX(float newPos) { targetPosOriginitionX = newPos; }

    public void SetCliffJump()
    {
        gravity = -4f;
        SetVelocity(new Vector2(0f, MathEquations.CalculateLaunchVelocity(transform.position, Vector2.zero, gravity, GameMaster.instance.screenTopEdge).y - 1));
        movementType = MovementType.HangGlideTransition;
        shadow.GetComponent<SpriteRenderer>().enabled = false;
        anim.Play("Shiba Double Jump");
    }
}