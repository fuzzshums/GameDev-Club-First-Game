using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 inactivePos;
    public Vector2 initialPos;
    public Vector2 pos;
    public bool firing;
    public bool fireRequest;
    public GameObject myPlayer;
    GameObject myMasterMind;


    // Use this for initialization
    public void Start () {
        myMasterMind = GameObject.Find("Master Mind");
        firing = false;
        fireRequest = false;
        inactivePos = new Vector2(-10, -10);
        this.transform.position = inactivePos;
        myPlayer = GameObject.Find("Player");
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
            firing = true;
        }
    }

    public void fireBulletUpdate()
    {
        float step = 6;
        step = myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate();
        pos.y += step * (Time.deltaTime);
        this.transform.position = pos;
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
            resetPos();
        }
    }
}

