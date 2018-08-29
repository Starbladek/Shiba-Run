using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyPlayer : MonoBehaviour
{
    public float speed = 5;
    public float elasticStrength = 15;
    public float elasticDegradeRate = 0.95f;
    Vector2 targetPosOrigin;
    Vector2 targetPosSin;
    float ySinOffset;
    float ySinCounter;
    float xSinOffset;
    bool elasticActive;

    //Components
    Rigidbody2D rb;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ySinCounter += (2 * Time.deltaTime);
        if (ySinCounter >= 360) ySinCounter = ySinCounter - 360;
        ySinOffset = Mathf.Sin(ySinCounter) * 0.25f;
        xSinOffset = Mathf.Sin(ySinCounter * 2) * 0.15f;
        
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetPosOrigin += (directionalInput * speed) * Time.deltaTime;
        if (targetPosOrigin.x < GameMaster.instance.screenLeftEdge + 1.5f) targetPosOrigin = new Vector2(GameMaster.instance.screenLeftEdge + 1.5f, targetPosOrigin.y);
        if (targetPosOrigin.x > GameMaster.instance.screenRightEdge - 1.5f) targetPosOrigin = new Vector2(GameMaster.instance.screenRightEdge - 1.5f, targetPosOrigin.y);
        if (targetPosOrigin.y < GameMaster.instance.screenBottomEdge + 1) targetPosOrigin = new Vector2(targetPosOrigin.x, GameMaster.instance.screenBottomEdge + 1);
        if (targetPosOrigin.y > GameMaster.instance.screenTopEdge - 1) targetPosOrigin = new Vector2(targetPosOrigin.x, GameMaster.instance.screenTopEdge - 1);
        
        transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rb.velocity.y * 2);
    }

    void FixedUpdate()
    {
        /*if (elasticActive)
        {
            rb.AddForce(elasticStrength * (targetPosOrigin - new Vector2(transform.position.x, transform.position.y)));
            rb.velocity *= elasticDegradeRate;
            if (Vector2.Distance(targetPosOrigin, new Vector2(transform.position.x, transform.position.y)) < 0.1f && rb.velocity.magnitude < 0.1f)
            {
                elasticActive = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
        {
            if (Vector2.Distance(targetPosOrigin, new Vector2(transform.position.x, transform.position.y)) > 0.1f)
            {
                elasticActive = true;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }*/
        rb.AddForce(elasticStrength * (new Vector2(targetPosOrigin.x + xSinOffset, targetPosOrigin.y + ySinOffset) - new Vector2(transform.position.x, transform.position.y)));
        rb.velocity *= elasticDegradeRate;
    }

    public void SetTargetPosOrigin(Vector2 newVect) { targetPosOrigin = newVect; }
}