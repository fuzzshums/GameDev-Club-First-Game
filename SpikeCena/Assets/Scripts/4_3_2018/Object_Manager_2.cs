﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Object_Manager_2 : MonoBehaviour
{
    public GameObject myPlayer;
    //These are lists the Unity user will fill with game objects
    public List<GameObject> spikeList;
    public List<GameObject> bulletList;

    public int numSpikes;
    public int numBullets;
    public float timeScale;

    //These are lists ObjectManager will use to keep track of all the duplicated objects it has made
    List<GameObject> whiteSpikeList;
    List<GameObject> whiteBulletList;
    List<GameObject> yellowBulletList;
    List<GameObject> blueBulletList;
    List<GameObject> greenBulletList;

    //UI
    public Text timeText;
    float time = 0;

    //Spawn manager
    public static SpawnManager spawnManager; 

    // Use this for initialization
    void Start()
    {
        whiteSpikeList = new List<GameObject>();

        whiteBulletList = new List<GameObject>();
        yellowBulletList = new List<GameObject>();
        blueBulletList = new List<GameObject>();
        greenBulletList = new List<GameObject>();
        spawnSpikes();
        spawnBullets();
        timeText.text = "Seconds Alive: " + ((int)Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        clearOldObjects();
        time = time + Time.deltaTime;
        timeText.text = "Seconds Alive: " + ((int)time).ToString();
        Time.timeScale = timeScale;
    }

    void clearOldObjects()
    {
        //TODO bullets
        //TODO spikes
        for (int i = 0; i < whiteSpikeList.Count; i++)
        {
            if (whiteSpikeList[i].transform.position.y <= -4)
            {
                randomizeSpawn(whiteSpikeList[i]);
            }
        }
    }

    //SPIKES
    #region
    void spawnSpikes()
    {
        for (int i = 0; i < spikeList.Count; i++)
        {
            for (int j = 0; j < numSpikes; j++)
            {
                GameObject spike = Instantiate(spikeList[i]);
                
                float xPos = UnityEngine.Random.Range(-8.88f, 8.88f);
                float yPos = UnityEngine.Random.Range(10f, 20f);
                Vector2 startPos = new Vector2(xPos, yPos);
                spike.transform.position = startPos;
                whiteSpikeList.Add(spike); //TODO change when adding more spike types
            }
        }
    }
    void randomizeSpawn(GameObject spike)
    {
        float xPos = UnityEngine.Random.Range(-8.88f, 8.88f);
        float yPos = UnityEngine.Random.Range(6f, 9f);
        Vector2 newPos = new Vector2(xPos, yPos);
        spike.transform.position = newPos;
    }
    #endregion

    //BULLETS
    void spawnBullets()
    {
        for (int j = 0; j < numBullets; j++)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (i == 0)
                {
                    GameObject whiteBullet = Instantiate(bulletList[0]);
                    whiteBulletList.Add(whiteBullet);
                }
                else if (i == 1)
                {
                    GameObject yellowBullet = Instantiate(bulletList[1]);
                    yellowBulletList.Add(yellowBullet);
                }
                else if (i == 2)
                {
                    GameObject blueBullet0 = Instantiate(bulletList[2]);
                    GameObject blueBullet1 = Instantiate(bulletList[2]);
                    GameObject blueBullet2 = Instantiate(bulletList[2]);
                    blueBulletList.Add(blueBullet0);
                    blueBulletList.Add(blueBullet1);
                    blueBulletList.Add(blueBullet2);
                }
                else if (i == 3)
                {
                    GameObject greenBullet = Instantiate(bulletList[3]);
                    greenBulletList.Add(greenBullet);
                }
            }

        }
    }

    public void fireFreeBullet(int type)
    {
        if (type == 1)
        {
            for (int i = 0; i < yellowBulletList.Count; i++)
            {
                if (yellowBulletList[i].GetComponent<Bullet>().fireRequest == false)
                {
                    yellowBulletList[i].GetComponent<Bullet>().fireRequest = true;
                    return;
                }
            }
        }
        else if (type == 2)
        {
            for (int i = 0; i < blueBulletList.Count; i = i + 3)
            {
                if (blueBulletList[i].GetComponent<Bullet>().fireRequest == false)
                {
                    blueBulletList[i].GetComponent<Bullet>().fireRequest = true;
                    blueBulletList[i + 1].GetComponent<Bullet>().fireRequest = true;
                    blueBulletList[i + 1].GetComponent<BlueBullet>().direction = -1;
                    blueBulletList[i + 2].GetComponent<Bullet>().fireRequest = true;
                    blueBulletList[i + 2].GetComponent<BlueBullet>().direction = 1;
                    return;
                }
            }
        }
        else if (type == 3)
        {
            for (int i = 0; i < greenBulletList.Count; i++)
            {
                if (greenBulletList[i].GetComponent<Bullet>().fireRequest == false)
                {
                    greenBulletList[i].GetComponent<Bullet>().fireRequest = true;
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < whiteBulletList.Count; i++)
            {
                if (whiteBulletList[i].GetComponent<Bullet>().fireRequest == false)
                {
                    whiteBulletList[i].GetComponent<Bullet>().fireRequest = true;
                    return;
                }
            }
        }
        return;
        throw new Exception("No bullets left!");
    }
}