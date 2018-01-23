using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBullet : Bullet
{
    public int direction = 0;
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

        float step = 6;
        pos.y += step * (1 * Time.deltaTime);
        this.transform.position = pos;
        if (direction < 0)
        {
            pos.x += step / 2 * (1 * Time.deltaTime);
        }
        if (direction > 0)
        {
           pos.x -= step / 2 * (1 * Time.deltaTime);

        }
        if (this.transform.position.y > 6)
        {
            direction = 0;
            resetPos();
        }
    }
}
