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
    private int bulletCount;

    void Start () {
        myMasterMind = GameObject.Find("Master Mind");
        playerPosition = this.transform.position;
        currentBullet = 0;
        regColor = GetComponent<SpriteRenderer>().color;
        bulletCount = 0;
	}
	
	// Update is called once per frame yes test change code!
	void Update () {
        movementSpeed = .9f * myMasterMind.GetComponent<MasterMind>().getWhiteMovementRate(); //Scaling
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
        if (Input.GetMouseButtonDown(0) && currentBullet == 0) //NOTE: removed space for hotkey max view window
        {
            var targetPos = Input.mousePosition;
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            objectManager.GetComponent<Object_Manager_2>().fireFreeBullet(currentBullet, targetPos);
        }
        if (Input.GetMouseButtonDown(0) && currentBullet != 0)
        {
            objectManager.GetComponent<Object_Manager_2>().fireFreeBullet(currentBullet, Input.mousePosition);
            bulletCount++;
        }
        if (Input.GetMouseButtonDown(0) && bulletCount > 10)
        {
            bulletCount = 0;
            currentBullet = 0;
        }

    }

    private void Die()
    {
        myMasterMind.GetComponent<MasterMind>().increaseScore(-100);
        Debug.Log("You've died.");
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
        if (other.gameObject.tag == "Powerup")
        {
            other.gameObject.GetComponent<Powerup>().transform.position = new Vector2(-10f, -10f);
            myMasterMind.GetComponent<MasterMind>().increaseScore(100);
            currentBullet = 1;
            yield return new WaitForSeconds(5);
            other.gameObject.GetComponent<Powerup>().randomizePos(); 
        }
    }
}
