using System.Collections; //TEST BABY
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject objectManager;
    Vector2 playerPosition;
    public float movementSpeed;
    int currentBullet;

    void Start () {
        playerPosition = this.transform.position;
        int currentBullet = 0;
	}
	
	// Update is called once per frame yes test change code!
	void Update () {
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
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        {
            objectManager.GetComponent<ObjectManager>().fireFreeBullet(currentBullet);
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
}
