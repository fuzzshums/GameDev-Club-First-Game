using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject objectManager;
    GameObject myMasterMind;
    public int health;
    public float movementSpeed;

    //private vars
    private Vector2 playerPosition;
    private int currentBullet;
    private float flinchTime = 0.1f;     //time player color changes for when hit
    private Color damagedColor = Color.red;
    private Color regColor;

    void Start () {
        myMasterMind = GameObject.Find("Master Mind");
        playerPosition = this.transform.position;
        //int currentBullet = 0;
        regColor = GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame yes test change code!
	void Update () {
        movementSpeed = myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate();
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

    void checkInput()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if ((Input.GetKey("a") || Input.GetKey("left")) && screenPos.x > 25)
        {
            playerPosition.x -= movementSpeed * Time.deltaTime;
            this.transform.position = playerPosition;
        }
        if ((Input.GetKey("d") || Input.GetKey("right")) && screenPos.x < Screen.width - 25)
        {
            playerPosition.x += movementSpeed * Time.deltaTime;
            this.transform.position = playerPosition;
        }
        if (Input.GetMouseButtonDown(0)) //NOTE: removed space for hotkey max view window
        {
            var targetPos = Input.mousePosition;
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            objectManager.GetComponent<Object_Manager_2>().fireFreeBullet(currentBullet, targetPos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentBullet == 3) {
                currentBullet = 0;
            } else {
                currentBullet++; //changed for TortoiseSVN testing (was currentBullet++;) Looks Good! (Added in merge) - CHANGED BACK test
                
            }
        }
    }

    private void Die()
    {
        //Do dying stuff
        Debug.Log("You've died.");
    }

    //Shows visual indication that player is damaged for flinchTime seconds
    IEnumerator OnDamage()
    {
        GetComponent<SpriteRenderer>().color = damagedColor;
        yield return new WaitForSeconds(flinchTime);
        GetComponent<SpriteRenderer>().color = regColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            /*
            Spike spikeBehavior = other.gameObject.GetComponent<Spike>();
            health -= spikeBehavior.GetDamage();
            Debug.Log("" + health);
            if(health <= 0)
            {
                Die();
            }
            StartCoroutine(OnDamage());
            other.gameObject.GetComponent<Spike>().Die();
            */
            other.gameObject.GetComponent<SpikeWhite>().resetPos();
            Die();
        }
    }
}
