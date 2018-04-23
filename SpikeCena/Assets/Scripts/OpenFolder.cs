using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OpenFolder : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Path.GetFileName.html

    //https://answers.unity.com/questions/1381625/why-was-wwwaudioclip-removed.html
    //https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequestMultimedia.GetAudioClip.html
    GameObject musicManager;
    // Use this for initialization
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;

    public object WebRequestMultimedia { get; private set; }

    void Start()
    {
        musicManager = GameObject.Find("Music Master");

        string path = Application.dataPath;
        Debug.Log(path);
        string[] A = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
        string temp;
        //musicManager.GetComponent<AudioClip>().
        
        for (int i = 0; i < 1; i++)
        {
            temp = "Assets/" + A[i].Remove(0, path.Length);
            Debug.Log(temp);
            AssetDatabase.ImportAsset(temp);
            WebRequestMultimedia.GetAudioClip();
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
