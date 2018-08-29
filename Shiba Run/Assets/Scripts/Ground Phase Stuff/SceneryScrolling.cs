using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryScrolling : MonoBehaviour
{
    [HideInInspector]
    public bool active = false;
    public float scrollSpeed;
    [HideInInspector]
    public int numOfSegments;
    Vector2 chainEnd;
    bool cliffMode = false;

    public GameObject streetSegmentPrefab;

    public Sprite houseSpriteUnbroken;
    public Sprite houseSpriteBroken;
    public Sprite cliffSprite;

    public Sprite[] streetSprites;
    float spriteWidth;

    public Sprite backgroundNearSprite;
    public Sprite backgroundFarSprite;

    [HideInInspector]
    public List<GameObject> streetSegments = new List<GameObject>();



    void Start()
    {
        //I spent a while trying to figure out how to automatically track how many segments would fit on screen by taking the width of the screen and dividing
        //it by the width of the image (For example, a screen width of 1618 divided by a house segment width of 1280 means ~1.2 house segments could fit on
        //the screen at a given time. This would have allowed us to automatically determine how many segments should be in the chain, no matter what image
        //width we were working with. However, I couldn't get the code below to work, so we're just throwing in 12 segments because fuck it
        //print(Screen.width);                                  //Only prints 1618 while maximized on play, not when it isn't maximized
        //print(streetSprites[0].rect.size.x);                  //Always prints 1280
        //print(Screen.width / streetSprites[0].rect.size.x);   //I want it to ALWAYS get 1618, but it doesn't...

        spriteWidth = streetSprites[0].bounds.size.x;
        numOfSegments = 12;
        if ((numOfSegments % 2) == 1) numOfSegments++;   //Make sure the amount of buildings is always an even number

        //The startingPosX and Y set where the chain of building segments ends (off to the left of the screen). The basic setup is that the chain of
        //buildings extend 5 segments to the left, 1 on the screen, and 6 to the right.
        //I'm not exactly sure how to work with ScreenToWorldPoint, but I know for a fact that the code below gets the bottom-left corner of whatever
        //the camera sees from that Z distance of 7. We use this as our starting point to determine where the rest of the segments go
        chainEnd = new Vector2(
            GameMaster.instance.screenLeftEdge - (spriteWidth * ((numOfSegments / 2) - 1)),  //Left-most edge of the chain
            GameMaster.instance.screenBottomEdge                                             //Bottom edge of the chain
        );

        for (int i = 0; i < numOfSegments; i++)
        {
            GameObject streetSegment = Instantiate(streetSegmentPrefab);
            streetSegment.transform.parent = transform;
            streetSegment.transform.position = new Vector2(chainEnd.x + (spriteWidth * i), chainEnd.y);

            //If this segment is where the house should be, set the sprite as the house sprite
            //otherwise just set it to a random sprite
            streetSegment.GetComponent<SpriteRenderer>().sprite = (i == ((numOfSegments / 2) - 1)) ? houseSpriteUnbroken : streetSprites[Random.Range(0, 7)];
            streetSegments.Add(streetSegment);
        }
    }

    void Update()
    {
        if (active)
        {
            GameObject segmentToDelete = null;	//We have to do this because we can't delete/remove items from a list within a foreach loop going through it
            foreach (GameObject streetSegment in streetSegments)
            {
                streetSegment.transform.Translate(new Vector2(-(scrollSpeed * Time.deltaTime), 0));

                //If the segment goes off the left side of the chainEnd, set it to be deleted outside of this loop
                if (streetSegment.transform.position.x <= chainEnd.x - spriteWidth) segmentToDelete = streetSegment;
            }
            if (segmentToDelete != null)
            {
                streetSegments.Remove(segmentToDelete);
                Destroy(segmentToDelete);

                if (!cliffMode)
                {
                    GameObject newSegment = Instantiate(streetSegmentPrefab);
                    newSegment.transform.parent = transform;
                    newSegment.transform.position = new Vector2(
                        streetSegments[streetSegments.Count - 1].transform.position.x + spriteWidth,
                        streetSegments[streetSegments.Count - 1].transform.position.y
                    );
                    newSegment.GetComponent<SpriteRenderer>().sprite = streetSprites[Random.Range(0, 7)];
                    streetSegments.Add(newSegment);
                }
            }
        }
    }



    //Completely reset all segments
    public void reset()
    {
        cliffMode = false;
        LeanTween.cancel(gameObject);
        //transform.position = Vector2.zero;

		foreach (GameObject streetSegment in streetSegments) Destroy(streetSegment);
		streetSegments.Clear();

        for (int i = 0; i < numOfSegments; i++)
        {
            GameObject streetSegment = Instantiate(streetSegmentPrefab);
            streetSegment.transform.parent = transform;
            streetSegment.transform.position = new Vector2(chainEnd.x + (spriteWidth * i), chainEnd.y);
            streetSegment.GetComponent<SpriteRenderer>().sprite = streetSprites[Random.Range(0, 7)];
            streetSegments.Add(streetSegment);
        }
    }

    //Start cliff mode
    public void StartCliffMode()
    {
        cliffMode = true;

		//Get the index of the segment that will become the cliff sprite spawn, and then give that segment the cliff sprite
		int cliffSpriteSegmentIndex = (numOfSegments / 2) + 1;
		streetSegments[cliffSpriteSegmentIndex].GetComponent<SpriteRenderer>().sprite = cliffSprite;

		//Starting with the index of the segment at the end of the list (numOfSegments - 1), we scroll backwards through the list
		//until we hit the segment with the cliff sprite, removing each segment as we go
		int index = (numOfSegments - 1);	//The reason we have to start at the end is because starting at the beginning causes the index to be modified as we remove elements from the list
											//like removing index 1 would cause index 2 to become the new index 1

		while (index > cliffSpriteSegmentIndex)
		{
			Destroy(streetSegments[index]);	//This line destroys the segment, but leaves it as "null" in the list
			streetSegments.RemoveAt(index);	//This line removes the null element from the list
			index--;
		}
    }

	public void SetCliffMode(bool temp)
	{
		cliffMode = temp;
	}
}