using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Get the AudioSource we want to use to play our AudioClip.
        var source = this.GetComponent<AudioSource>();
        // Load an AudioClip from the streaming assets folder into our source.
        string path = "D:\\TestAudio\\Big Wild - Invincible (feat. iDA HAWK)";
        source.clip = ES3.LoadAudio(path);
        // Play the AudioClip we just loaded using our AudioSource.
        source.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
