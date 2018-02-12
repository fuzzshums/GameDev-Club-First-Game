﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : Bullet
{
    float timeCounter = 0;
    bool activateSpin = false;
    float circleSize = 1;

    // Update is called once per frame
    new void Update()
    {
        checkFireRequest();

        if (firing)
        {
            fireBulletUpdate();
        }
    }

    new void fireBulletUpdate()
    {
        if (activateSpin)
        {
            bulletSpin();
        }
        else if(!activateSpin)
        {
            float step = 4;
            pos.y += step * (1 * Time.deltaTime);
            this.transform.position = pos;
            if (this.transform.position.y > 1)
            {
                activateSpin = true;
            }
        }
    }
    
    void bulletSpin()
    {
        timeCounter += Time.deltaTime;
        circleSize += Time.deltaTime;
        float x = ((Mathf.Cos(timeCounter * 2)) * circleSize) + initialPos.x - (circleSize);
        float y = ((Mathf.Sin(timeCounter * 2)) * circleSize );
        this.transform.position = new Vector2(x, y);
        
        if (this.transform.position.y > 6)
        {
           activateSpin = false;
           timeCounter = 0;
           circleSize = 1;
           resetPos();
        }
    }
}
