using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    Vector2 randomPos;
    Random rnd = new Random();
    

	// Use this for initialization
	void Start () {
        randomizePos();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void randomizePos()
    {
        randomPos = new Vector2(Random.Range(-8.0f, 6.0f) ,-4.3f);
        this.transform.position = randomPos;
    }

  
}
