using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    
    public void exitGame()
    {
        Application.Quit();
    }
}
 