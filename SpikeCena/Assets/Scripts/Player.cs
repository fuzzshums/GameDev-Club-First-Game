﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public GameObject objectManager;
    GameObject myMasterMind;
    public int health;
    public float movementSpeed;
    
    //private vars
    private int currentBullet;
    private float flinchTime = 0.1f;     //time player color changes for when hit
    private Color damagedColor = Color.red;
    private Color regColor;
    private int bulletCount;
    private Rigidbody2D playerRB;
    private Vector2 playerVelocity;

    private int damagePenalty;
    private int powerupPoints;

    //scripts
    MasterMind mmScript;

    void Start () {
        myMasterMind = GameObject.Find("Master Mind");
        mmScript = myMasterMind.GetComponent<MasterMind>();
        currentBullet = 0;
        regColor = GetComponent<SpriteRenderer>().color;
        bulletCount = 0;

        playerRB = GetComponent<Rigidbody2D>();
        playerVelocity = new Vector2(0f, 0f);
        MasterMind mm = myMasterMind.GetComponent<MasterMind>();
        damagePenalty = mm.damagePenalty;
        powerupPoints = mm.powerupPoints;
	}
	
	// Update is called once per frame yes test change code!
	void Update () {
        movementSpeed = 1f + .75f * myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate(); //Scaling
        if (movementSpeed > myMasterMind.GetComponent<MasterMind>().playerRateCap) //TODO remove these out of update place at top -UNLESS WE WANT TO DYNAMIC UPDATE
        {
            movementSpeed = myMasterMind.GetComponent<MasterMind>().playerRateCap;
        }
        if (movementSpeed < .5f)
        {
            movementSpeed = .5f;
        }
        checkInput();
    }

    private void FixedUpdate()
    {
        playerRB.velocity = playerVelocity;
    }

    void checkInput()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            playerVelocity.Set(-movementSpeed, 0);
        }
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            playerVelocity.Set(movementSpeed, 0);
        }
        else
        {
            playerVelocity.Set(0, 0);
        }
        if ((Input.GetMouseButtonDown(0)) && bulletCount > 0) 
        {
            objectManager.GetComponent<Object_Manager_2>().fireFreeBullet(0, Input.mousePosition);
            myMasterMind.GetComponent<MasterMind>().decreaseAmmo();
            bulletCount--;
        }
        if ((Input.GetMouseButtonDown(1)) && bulletCount > 0)
        {
            objectManager.GetComponent<Object_Manager_2>().fireFreeBullet(1, Input.mousePosition);
            myMasterMind.GetComponent<MasterMind>().decreaseAmmo();
            bulletCount--;
        }
        if (Input.GetKeyDown("p")) 
        {
            myMasterMind.GetComponent<MasterMind>().pause();

        }
        if (Input.GetKeyDown("escape"))
        {
            myMasterMind.GetComponent<MasterMind>().finalizeStats();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
   
    //Shows visual indication that player is damaged for flinchTime seconds
    IEnumerator OnDamage()
    {
        GetComponent<SpriteRenderer>().color = damagedColor;
        yield return new WaitForSeconds(flinchTime);
        GetComponent<SpriteRenderer>().color = regColor;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            other.gameObject.GetComponent<SpikeWhite>().resetPos();
            myMasterMind.GetComponent<MasterMind>().modifyHealth(-1);
            myMasterMind.GetComponent<MasterMind>().modifyScore(damagePenalty);
            health--;
            if (health <= 0)
            {
                myMasterMind.GetComponent<MasterMind>().finalizeStats();
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
            StartCoroutine(OnDamage());
        }
        else if (other.gameObject.tag == "Powerup")
        {
            other.gameObject.GetComponent<Powerup>().transform.position = new Vector2(-10f, -10f);
            myMasterMind.GetComponent<MasterMind>().modifyScore(powerupPoints);
            mmScript.increasePowPickedUp();

            if (bulletCount + 10 <= 50)
            {
                bulletCount += 10;
                mmScript.increaseAmmo();
            }

            yield return new WaitForSeconds(5);
            other.gameObject.GetComponent<Powerup>().randomizePos(); 
        }
        else if (other.gameObject.tag == "Health")
        {
            other.gameObject.GetComponent<Powerup>().transform.position = new Vector2(-10f, -10f);
            myMasterMind.GetComponent<MasterMind>().modifyScore(powerupPoints);
            mmScript.increasePowPickedUp();
            health++;
            mmScript.modifyHealth(1);
            yield return new WaitForSeconds(10);
            other.gameObject.GetComponent<Powerup>().randomizePos();
        }
    }
}
