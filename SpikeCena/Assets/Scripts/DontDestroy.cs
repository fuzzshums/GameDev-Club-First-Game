using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour {

    // Use this for initialization
    public string userSongChoice;
	void Start () {
        Destroy(GameObject.Find("OpenFolder"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
