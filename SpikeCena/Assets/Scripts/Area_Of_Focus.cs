using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Of_Focus : MonoBehaviour {

    // Use this for initialization
    public GameObject myPlayer;
    public GameObject myMasterMind;
    static public float xPos;
    static public float xScale;
    static public int choice;
    string direction;
    float directionTimeSwitch;
    float directionInterval;
    float timeSwitch;
    float interval;
	void Start () {
		myPlayer = GameObject.Find("Player");
        myMasterMind = GameObject.Find("Master Mind");
        direction = "left";
        timeSwitch = 0;
        interval = 12;
        choice = 0;
        directionInterval = 6;
        directionTimeSwitch = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timeSwitch += Time.deltaTime;
        if (timeSwitch > interval)
        {
            Debug.Log("Switching AoF action!");
            timeSwitch = 0;
            choice = UnityEngine.Random.Range(0, 3);
            if (choice == 1)
            {
                directionTimeSwitch = 0;
            }
        }
        decide_action(choice);
    }

    void decide_action(int choice)
    {
        if (choice == 0)
        {
            follow_player();
        }
        else if (choice == 1)
        {
            move_freely();
        }
        else if (choice == 2)
        {
            full_screen();
        }
    }

    void move_freely()
    {
        directionTimeSwitch += Time.deltaTime;
        if (directionTimeSwitch > directionInterval) {
            directionTimeSwitch = 0;
            int temp_direction = UnityEngine.Random.Range(0, 2);
            if (temp_direction == 0)
            {
                direction = "right";
            }
            else
            {
                direction = "left";
            }
        }

        if (direction == "left")
        {
            xPos = this.transform.position.x - .75f * Time.deltaTime * myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate();
            xScale = 6;
            if (xPos - xScale/2 < -8.88f) // < bounds
            {
                Debug.Log("Bound detected!");
                direction = "right";
            }
            this.transform.localScale = new Vector2(xScale, this.transform.localScale.y);
            this.transform.position = new Vector3(xPos, this.transform.position.y, this.transform.position.z);
        }
        else if (direction == "right")
        {
            xPos = this.transform.position.x + .5f * Time.deltaTime  * myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate();
            xScale = 6;
            if (xPos + xScale/2 > 8.88f) // < bounds
            {
                Debug.Log("Bound detected!");
                direction = "left";
            }
            this.transform.localScale = new Vector2(xScale, this.transform.localScale.y);
            this.transform.position = new Vector3(xPos, this.transform.position.y, this.transform.position.z);
        }
    }
    void full_screen()
    {
        float currentXPos = this.transform.position.x;
        xPos = Mathf.Lerp(currentXPos, 0, .1f);
        float currentXScale = this.transform.localScale.x;
        xScale = Mathf.Lerp(currentXScale, 8.88f*2, .1f);
        this.transform.position = new Vector3(xPos, this.transform.position.y, this.transform.position.z);
        this.transform.localScale = new Vector2(xScale, this.transform.localScale.y);
    }

    void follow_player()
    {
        float currentX = this.transform.position.x;
        float playerX = myPlayer.transform.position.x;
        xPos = Mathf.Lerp(currentX, playerX, .01f * (1 + myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate()));
        float currentXScale = this.transform.localScale.x;
        xScale = Mathf.Lerp(currentXScale, 6, .1f);
        this.transform.localScale = new Vector2(xScale, this.transform.localScale.y);
        float yPos = this.transform.position.y;
        float zPos = this.transform.position.z;
        this.transform.position = new Vector3(xPos, yPos, zPos);
    }

    public float getPosition()
    {
        return xPos;
    }
    public float getScale()
    {
        return xScale;
    }
    public void setChoice(int value)
    {
        choice = value;
    }
}
