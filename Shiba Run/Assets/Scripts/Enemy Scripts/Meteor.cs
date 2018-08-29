using UnityEngine;

public class Meteor : MonoBehaviour
{
    Animator anim;
    public AnimationClip meteorSmoke;

    GameObject meteorShadow;
    public Sprite meteorShadowSprite;
    GameObject meteorHeat;
    public Sprite meteorHeatSprite;

    BoxCollider2D hitBox;

    public float speed;
    public float fallTimerLength;
    [HideInInspector]
    public float hitPosX;
    public float impactShakeIntensity;
    public float impactShakeDuration;

    void Start()
    {
        //print(GameObject.Find("Phase Handler").GetComponent<PhaseHandler>().music.time);
        //Create and animate the meteor shadow
        meteorShadow = new GameObject();
        meteorShadow.name = "Meteor Shadow";
        meteorShadow.tag = "Junk";
        meteorShadow.transform.position = new Vector2(hitPosX, -0.67f);
        meteorShadow.transform.localScale = new Vector2(0.2f, 0.2f);
        meteorShadow.AddComponent<SpriteRenderer>().sprite = meteorShadowSprite;
        meteorShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        LeanTween.scale(meteorShadow, new Vector2(1.25f, 1.25f), fallTimerLength + 0.1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.alpha(meteorShadow, 1f, fallTimerLength + 0.1f).setEase(LeanTweenType.easeOutCubic);

        //Create and animate the meteor heat
        meteorHeat = new GameObject();
        meteorHeat.name = "Meteor Heat";
        meteorHeat.tag = "Junk";
        meteorHeat.transform.position = meteorShadow.transform.position;
        meteorHeat.transform.localScale = new Vector2(1f, 0.25f);
        meteorHeat.AddComponent<SpriteRenderer>().sprite = meteorHeatSprite;
        meteorHeat.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        LeanTween.scaleY(meteorHeat, 2f, fallTimerLength + 0.1f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.alpha(meteorHeat, 0.5f, fallTimerLength + 0.1f).setEase(LeanTweenType.easeInOutCubic);

        //Wait a set amount of time before creating the actual meteor
        LeanTween.delayedCall(gameObject, fallTimerLength, () =>
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            Vector3 dir = new Vector3(hitPosX, -0.67f) - transform.position;            //Get the direction to the object we're pointing at
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                    //Convert that direction into an angle
            transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);    //Rotate on the Vector3.forward (z) axis

            LeanTween.move(gameObject, new Vector2(transform.position.x - (transform.position.x - hitPosX), -0.85f), 0.1f).setOnComplete(meteorImpact);
        });

        anim = GetComponent<Animator>();
    }

    void meteorImpact()
    {
        //Reset rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //Play the smoke animation
        anim.Play("Meteor Smoke");

        //Create the meteor hitbox
        hitBox = gameObject.AddComponent<BoxCollider2D>();
        hitBox.isTrigger = true;
        hitBox.size = new Vector2(1f, 2f);
        //hitBox.size = new Vector2(gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x, gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        hitBox.offset = new Vector2(0, 1f);
        Destroy(hitBox, Time.deltaTime * 4f);

        //Destroy the meteor shadow
        Destroy(meteorShadow);

        //Destroy the meteor heat
        Destroy(meteorHeat);

        //Destroy the whole meteor after the animation is done playing
        LeanTween.delayedCall(gameObject, meteorSmoke.length, () =>
        {
            Destroy(gameObject);
        });

        //Play sound (temporary)
        GetComponent<AudioSource>().Play();
    }
}
