using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//todo bullet counter
//todo song_over bool
//todo average music intensity
//todo spike diagonal

// score vs. health
// power up -> ammo
// UI ammo  & song over screen

public class OpenFolder : MonoBehaviour
{

    public InputField userText;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        updateUserPressedEnter();
    }

    void updateUserPressedEnter()
    {
        if (Input.GetKeyDown("return"))
        {
            string path = userText.text;
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                Debug.Log(file);
            }
        }
    }
}
