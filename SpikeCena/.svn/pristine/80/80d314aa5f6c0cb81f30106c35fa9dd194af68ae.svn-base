using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : Bullet
{
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
        if (this.transform.position.y > 6)
        {
            resetPos();
        }
    }
}
