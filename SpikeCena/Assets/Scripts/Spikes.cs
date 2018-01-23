using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    // Use this for initialization
    public float xspeed;
    public float yspeed;
    Vector3 pointing;
    // spike color, first letter of each color
    // red = r, blue = b, white = w, ect...
    public char color;

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // just some random stuff to demonstrate movement
        if (color == 'w')
        {
            yspeed = Time.timeSinceLevelLoad * -.3f;
            xspeed = Mathf.Sin(yspeed * 10) * 5;
        }
        // changing direction and moving
        pointing = new Vector3(0, 0, (Mathf.Atan2(yspeed, xspeed) * Mathf.Rad2Deg) - 90);
        transform.eulerAngles = pointing;
        transform.Translate(xspeed * Time.deltaTime, yspeed * Time.deltaTime, 0, Space.World);
	}
}
