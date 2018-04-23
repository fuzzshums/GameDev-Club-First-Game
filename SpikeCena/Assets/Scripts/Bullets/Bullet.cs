using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 inactivePos;
    public Vector2 initialPos;
    public Vector2 pos;
    public Vector2 targetPos;
    public bool firing;
    public bool fireRequest;
    public GameObject myPlayer;
    GameObject myMasterMind;

    private int hitSpikePoints;
 
    // Use this for initialization
    public void Start () {
        myMasterMind = GameObject.Find("Master Mind");
        firing = false;
        fireRequest = false;
        inactivePos = new Vector2(-10, -10);
        this.transform.position = inactivePos;
        myPlayer = GameObject.Find("Player");
        hitSpikePoints = myMasterMind.GetComponent<MasterMind>().hitSpikePoints;
    }

    // Update is called once per frame
    public void Update () {
       checkFireRequest();
       if (firing) {
           fireBulletUpdate();
        }

    }
  
    public void checkFireRequest()
    {
        if (fireRequest && !firing) {

            initialPos = myPlayer.transform.position;
            pos = initialPos;
            //targetPos = Input.mousePosition;
            this.transform.position = myPlayer.transform.position;
            firing = true;
        }
    }

    public void fireBulletUpdate()
    {
        float step = 6;
        step = myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate() * Time.deltaTime;
        pos.y += step;
        this.transform.position = pos;
        //this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, step);
        if (this.transform.position.y > 6)
        {
            resetPos();
        }
    }

    public void resetPos()
    {
        firing = false;
        fireRequest = false;
        this.transform.position = inactivePos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            other.GetComponent<SpikeWhite>().resetPos();
            myMasterMind.GetComponent<MasterMind>().increaseScore(hitSpikePoints);            
            resetPos();
        }
    }
}

