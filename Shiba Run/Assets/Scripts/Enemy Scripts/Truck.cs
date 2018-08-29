using System.Collections;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public SimpleObject genericRubble;
    public TruckWheel truckWheel;
    public TruckFootball truckFootball;
    public Baby babyObject;

    void Start()
    {
        transform.position = new Vector2(GameMaster.instance.screenLeftEdge - GetComponent<SpriteRenderer>().bounds.size.x, 0);
        LeanTween.moveX(gameObject, GameMaster.instance.screenRightEdge - 3.4f, 2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            StartCoroutine("throwObjectsSequence");
        });
    }

    IEnumerator throwObjectsSequence()
    {
        Instantiate(genericRubble, transform.position, Quaternion.identity).LaunchObject(new Vector2(GameMaster.instance.screenLeftEdge, 0), GameMaster.instance.screenTopEdge - 1);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        Instantiate(truckWheel, new Vector2(transform.position.x, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        Instantiate(genericRubble, transform.position, Quaternion.identity).LaunchObject(new Vector2((GameMaster.instance.screenLeftEdge + GameMaster.instance.screenRightEdge) / 2f, 0), GameMaster.instance.screenTopEdge - 1);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        Instantiate(truckFootball, new Vector2(transform.position.x, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        Instantiate(genericRubble, transform.position, Quaternion.identity).LaunchObject(new Vector2(GameMaster.instance.screenLeftEdge, 0), GameMaster.instance.screenTopEdge - 1);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        Instantiate(babyObject, new Vector2(transform.position.x, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength);
        LeanTween.moveX(gameObject, GameMaster.instance.screenRightEdge, 1.5f).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}