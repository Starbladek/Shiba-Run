﻿using System.Collections;
using UnityEngine;

public class GliderTransition : MonoBehaviour
{
    public float waitHeight;
    public float driftSpeed;
    float prevY;

    GameObject player;
    bool hitPlayer = false;
    PhaseHandler ph;
    GameObject cloudFade;



    void Start()
    {
        ph = (PhaseHandler)FindObjectOfType(typeof(PhaseHandler));
        cloudFade = GameObject.Find("Cloud Fade");
        transform.position = new Vector2(0f.AddRandVal(-5, -2), GameMaster.instance.screenBottomEdge);
        transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 25);
        LeanTween.moveY(gameObject, waitHeight, 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.rotate(gameObject, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutCubic);
    }

    void Update()
    {
        transform.Translate(new Vector2(driftSpeed * Time.deltaTime, 0), Space.World);
        if (hitPlayer) player.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            hitPlayer = true;
            StartCoroutine(FlyUpRoutine());
        }
    }

    IEnumerator FlyUpRoutine()
    {
        LeanTween.moveY(gameObject, 8, 1f).setEase(LeanTweenType.easeInCubic);
        LeanTween.rotate(gameObject, new Vector3(0, 0, 30), 1f).setEase(LeanTweenType.easeInCubic);
        yield return new WaitForSeconds(0.5f);

        LeanTween.moveY(ph.groundScenery.gameObject, ph.groundScenery.transform.position.y - 5, 0.85f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveY(cloudFade, 0, 0.85f).setEase(LeanTweenType.easeInQuad);
        yield return new WaitForSeconds(0.85f);

        Destroy(player.GetComponent<GroundPlayer>().shadow);
        Destroy(player);
        Destroy(gameObject);
    }
}