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
   // public Text displayText;
    public GameObject ButtonPrefab;
    public GameObject canvasParent;

    public string magicString;
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
            //displayText.text = "";
            int counter = 0;
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                //Debug.Log(file);
                GameObject sample = Instantiate(ButtonPrefab);
                sample.transform.SetParent(canvasParent.transform, false);
                Vector2 newPos = sample.transform.position;
                newPos.y -= 75*(counter+1);
                sample.transform.position = newPos;
                sample.GetComponentInChildren<Text>().text = shortenfile(path,file);
                sample.GetComponent<Button>().onClick.AddListener(() => rememberName(file));
                counter += 1;
            }
        }
    }

    string shortenfile(string path, string file)
    {
        return file.Replace(path, "");
    }

    void rememberName(string file)
    {
        magicString = file;
        DontDestroyOnLoad(this.gameObject);
    }
}
