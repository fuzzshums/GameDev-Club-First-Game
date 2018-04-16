﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class will manage all of the variables of everything else.
//It will have getters that every other class can call to find out info.
//Everything communicates through mastermind.
public class MasterMind : MonoBehaviour {

    GameObject objectManager;
    GameObject musicManager;
    GameObject player;

    //View Only's
    public float VO_rate;
    //Use to modify params of other classes
    public float rateCap;
    public float playerRateCap;

    float oldNumSpikes;
    float newNumSpikes;
    float numSpikeThreshold;
    float timeWithinThreshold;
    float timeCap;
    float totalTime;
	// Use this for initialization
	void Start () {
        objectManager = GameObject.Find("Object Manager");
		musicManager = GameObject.Find("Music Master");
        player = GameObject.Find("Player");

        //Variables
        rateCap = 12f;
        playerRateCap = 12f;

        numSpikeThreshold = 1f;
        timeWithinThreshold = 0;
        timeCap = .1f;
        totalTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        totalTime += Time.deltaTime;
	}
    //1.) @@@@@@   SPIKES   @@@@@@
    #region
    public float getWhiteMovementRate()
    {
        float dF = 7f / 4f;
        float oldRate = VO_rate;
        float newRate = .5f + 4f * musicManager.GetComponent<MusicTest>().getIntensity(); //was 7
        if (newRate > rateCap)
        {
            newRate = rateCap;
        }
        if (newRate * 4f / dF < oldRate && newRate * 6f / dF >= oldRate)
        {
            Debug.Log("PLAYER mid slowing");
            VO_rate = Mathf.Lerp(oldRate, newRate, .2f);

        }
        else if (newRate * 6f  / dF < oldRate)
        {
            Debug.Log("PLAYER GOD slowing");
            VO_rate = Mathf.Lerp(oldRate, newRate, 1f);

        }
        else if (newRate < oldRate)
        {
            //Dampen the speed decrease!
            VO_rate = oldRate - (.25f / dF) * Time.deltaTime;
        }
        else //if its super LOW or greater than -> just lerp it
        {
            VO_rate = Mathf.Lerp(oldRate, newRate, .5f);
        }
        return VO_rate;
    }
    public int getNumSpikes()
    {
        float Scale = totalTime / 100;
        if (Scale < 1)
        {
            Scale = 1;
        }
        else if (Scale > 2)
        {
            Scale = 2;
        }
        else
        {
            //Debug.Log(Scale);
        }
        newNumSpikes = .75f + Scale * 12f * musicManager.GetComponent<MusicTest>().getIntensity();


        if (Mathf.Abs(newNumSpikes - oldNumSpikes) > numSpikeThreshold && timeWithinThreshold > timeCap)
        {
            timeWithinThreshold = 0;
            if (newNumSpikes > oldNumSpikes + 2)
            {
                oldNumSpikes += 2; //don't give away all our spikes at once!
                return (int)oldNumSpikes;
            }

            if (newNumSpikes + 2 < oldNumSpikes)
            {
                oldNumSpikes -= 2;
                return (int)oldNumSpikes;
            }
            else if (newNumSpikes < oldNumSpikes)
            {
                oldNumSpikes -= 1;
                return (int) oldNumSpikes;
            }
        }
        else
        {
            timeWithinThreshold += Time.deltaTime;
            newNumSpikes = oldNumSpikes;
        }
        oldNumSpikes = newNumSpikes;
        return (int) newNumSpikes;
    }
    #endregion
    //2.) @@@@@@   PLAYER   @@@@@@
    #region
    #endregion
}
