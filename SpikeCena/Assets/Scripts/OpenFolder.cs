using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;

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
        // ############ GATHER USER AUDIO DATA STRINGS ############
        string path = Application.dataPath;
        Debug.Log(path);
        string[] A = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
        
        for (int i = 0; i < A.Length; i++)
        {
            A[i] = A[i].Replace("/", "\\");
            Debug.Log(A[i]);

        }

        // Open the stream and read it back.
        using (FileStream fs = File.Open(A[0], FileMode.Open, FileAccess.Read))
        {
            float[] data;
            data = new float[1000];
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            Debug.Log(fs.Length);
            

            
            //public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream); 
            AudioClip test = AudioClip.Create("Test", samplerate * 2, 1, samplerate, true);
            Debug.Log("Init = " + test.length);
            int counter = 0;
            while (fs.Read(b, 0, b.Length) > 0)
            {
                if (counter < 1000)
                {
                    //Debug.Log(Encoding.Default.GetString(b));
                    //Debug.Log(myFloat);
                    //Console.WriteLine(Encoding.Default.GetString(value));
                    data[counter] = System.BitConverter.ToSingle(b, counter);

                    //Debug.Log(data[counter]);
                    //test.SetData(data, counter);
                }
                counter += 1;
            }
            test.SetData(data, 0);
            Debug.Log("Post = " + test.length);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
