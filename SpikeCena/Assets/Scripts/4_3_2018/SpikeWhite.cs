using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWhite : MonoBehaviour {

    public float rate;
    GameObject myMusicManager;

    private float rateCap;
    // Use this for initialization
    void Start () {
        rate = 3;
        rateCap = 14f;
        myMusicManager = GameObject.Find("Music Master");
    }
	
	// Update is called once per frame
	void Update () {
        float oldRate = rate;
        float newRate = .5f + 7f*myMusicManager.GetComponent<MusicTest>().getIntensity();
        if (newRate > rateCap)
        {
            newRate = rateCap;
        }
        if (newRate * 4f < oldRate && newRate * 6f >= oldRate)
        {
            Debug.Log("mid slowing");
            rate = Mathf.Lerp(oldRate, newRate, .2f);

        }
        else if (newRate * 6f < oldRate)
        {
            Debug.Log("GOD slowing");
            rate = Mathf.Lerp(oldRate, newRate, 1f);

        }
        else if (newRate < oldRate)
        {
            //Dampen the speed decrease!
            rate = oldRate - .25f * Time.deltaTime;
        }
        /*
        else if (newRate < oldRate * 2 && newRate >= oldRate * 4)
        {
            //Super Dampen the speed decrease!
            rate = oldRate - 2f * Time.deltaTime;
        }
        else if (newRate < oldRate * 4 && newRate >= oldRate * 8)
        {
            //HECKA Dampen the speed decrease!
            rate = oldRate - 4f * Time.deltaTime;
        }
        */
        else //if its super LOW or greater than -> just lerp it
        {
            rate = Mathf.Lerp(oldRate, newRate, .5f);
        }
        if (rate < .5f)
        {
            rate = .5f;
        }
        if (rate > rateCap)
        {
            rate = rateCap;
        }
        //rate = newRate;
        moveDown();
	}

    void moveDown()
    {
        Vector2 currentPos = this.transform.position;
        currentPos.y -= Time.deltaTime * rate;
        this.transform.position = currentPos;
    }

    public void resetPos()
    {
        float xPos = UnityEngine.Random.Range(-8.88f, 8.88f);
        float yPos = UnityEngine.Random.Range(6f, 9f);
        Vector2 newPos = new Vector2(xPos, yPos);
        this.transform.position = newPos;
    }
}
