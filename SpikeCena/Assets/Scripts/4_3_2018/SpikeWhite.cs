using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWhite : MonoBehaviour {

    public float rate;
    GameObject myMasterMind;

    private float rateCap;
    // Use this for initialization
    void Start () {
        rate = 3;
        rateCap = 10f;
        myMasterMind = GameObject.Find("Master Mind");
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
