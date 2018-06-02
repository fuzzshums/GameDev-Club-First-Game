using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;
using UnityEngine;
using System;
using System.Text;

//todo bullet counter
//todo song_over bool
//todo average music intensity
//todo spike diagonal

// score vs. health
// power up -> ammo
// UI ammo  & song over screen
// put score in the stats!!!!

public class OpenFolder : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Path.GetFileName.html

    //https://answers.unity.com/questions/1381625/why-was-wwwaudioclip-removed.html
    //https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequestMultimedia.GetAudioClip.html
    //https://answers.unity.com/questions/737002/wav-byte-to-audioclip.html
    GameObject musicManager;

    void Start()
    {
        musicManager = GameObject.Find("Music Master");
        /* @@@@@@@@@@@@@@ - COMMENTED OUT START
        // ############ GATHER USER AUDIO DATA STRINGS ############
        //string path = Application.dataPath;
        string path = "D:/TestAudio";
        Debug.Log(path);
        string[] A = Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories);

        for (int i = 0; i < 1; i++)
        {
            A[i] = A[i].Replace("/", "\\");
            var source = this.GetComponent<AudioSource>();
            // Load an AudioClip from the streaming assets folder into our source.
            source.clip = ES3.LoadAudio(A[i]);
            // Play the AudioClip we just loaded using our AudioSource.
            source.Play();
            Debug.Log(A[i]);

        }
        */ // @@@@@@@@@@@@@@@@@@ - COMMENTED OUT END
    }

    // Update is called once per frame
    void Update()
    {

    }
}
