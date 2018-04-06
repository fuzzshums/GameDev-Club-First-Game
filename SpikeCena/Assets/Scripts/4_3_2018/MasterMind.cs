using System.Collections;
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
	// Use this for initialization
	void Start () {
        objectManager = GameObject.Find("Object Manager");
		musicManager = GameObject.Find("Music Master");
        player = GameObject.Find("Player");

        //Variables
        rateCap = 12f;
        playerRateCap = 12f;
    }
	
	// Update is called once per frame
	void Update () {
		
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
    #endregion
    //2.) @@@@@@   PLAYER   @@@@@@
    #region
    #endregion
}
