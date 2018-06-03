using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    Vector2 randomPos;
    Random rnd = new Random();
    public float rate;
    GameObject myMasterMind;

    // Use this for initialization
    void Start () {
        myMasterMind = GameObject.Find("Master Mind");

        randomizePos();
    }
	
	// Update is called once per frame
	void Update () {
        rate = myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate();
        if (rate > myMasterMind.GetComponent<MasterMind>().rateCap) //TODO remove these out of update place at top -UNLESS WE WANT TO DYNAMIC UPDATE
        {
            rate = myMasterMind.GetComponent<MasterMind>().rateCap;
        }
        if (rate < .5f)
        {
            rate = .5f;
        }
        moveDown();
    }

    void moveDown()
    {
        Vector2 currentPos = this.transform.position;
        if (currentPos.y > -4.26f)
        {
            currentPos.y -= Time.deltaTime * rate;
            this.transform.position = currentPos;
        }
    }

    public void randomizePos()
    {
        randomPos = new Vector2(Random.Range(-8.0f, 6.0f) , 6f);
        this.transform.position = randomPos;
    }

  
}
