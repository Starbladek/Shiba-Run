using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    float speed;

    void Start()
    {
        speed = (Random.Range(0, 5) == 0) ? 0 : Random.Range(0f, 20f); //20% chance to be 0
        StartCoroutine(SwoopingAction());
    }

    void Update()
    {
        transform.Translate(new Vector2(-speed, 0) * Time.deltaTime);
    }

    IEnumerator SwoopingAction()
    {
        LeanTween.moveY(gameObject, Random.Range(0.25f, 0.5f), GameMaster.instance.quarterNoteLength * 2).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength * 2);

        LeanTween.moveY(gameObject, GameMaster.instance.screenTopEdge + 0.5f, GameMaster.instance.quarterNoteLength * 2).setEase(LeanTweenType.easeInCubic);
        yield return new WaitForSeconds(GameMaster.instance.quarterNoteLength * 2);

        Destroy(gameObject);
    }
}