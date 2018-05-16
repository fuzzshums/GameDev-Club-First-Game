using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWhite : MonoBehaviour {

    public float rate;
    public GameObject AoF_Manager;
    GameObject myMasterMind;
    float specialBoost;

    private float rateCap;
    float middle_of_object;
    // Use this for initialization
    void Start () {
        rate = 3;
        rateCap = 10f;
        myMasterMind = GameObject.Find("Master Mind");
        specialBoost = 1;
        resetPos(); //call on self @ start
    }
	
	// Update is called once per frame
	void Update () {
        rate = myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate()  * specialBoost;
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
        float chance = UnityEngine.Random.Range(0, 10);
        float xPos;
        if (chance < 7)
        {
            middle_of_object = AoF_Manager.GetComponent<AoF_Manager>().chooseAOF();
            float side_Range = AoF_Manager.GetComponent<AoF_Manager>().chooseAOF2(); // AOFs[value].GetComponent<Area_Of_Focus>().getScale()/2; //AoF_Manager.GetComponent<AoF_Manager>().choose_AoF().GetComponent<Area_Of_Focus>().getScale()/2;

            xPos = UnityEngine.Random.Range(middle_of_object - side_Range, middle_of_object + side_Range);
            specialBoost = 1.25f;
        }
        else
        {
            xPos = UnityEngine.Random.Range(-8.88f, 8.88f);
            specialBoost = 1f;
        }
        
        float yPos = UnityEngine.Random.Range(5.51f, 7f);
        Vector2 newPos = new Vector2(xPos, yPos);
        this.transform.position = newPos;
    }

}
