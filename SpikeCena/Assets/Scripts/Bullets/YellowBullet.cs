using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBullet : Bullet {

    bool right = true;
    bool left = false;

    // Update is called once per frame
    new void Update () {
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
        if (right) {
            pos.x += step * (1 * Time.deltaTime);
        }
        if (left) {
            pos.x -= step * (1 * Time.deltaTime);
        }
        if (pos.x > initialPos.x + 2) {
            left = true;
            right = false;
        }
        if (pos.x < initialPos.x - 2) {
            right = true;
            left = false;
        }
        this.transform.position = pos;
        if (this.transform.position.y > 6)
        {
            resetPos();
            right = true;
            left = false;
        }
    }

    new void checkFireRequest()
    {
        if (fireRequest && !firing)
        {
            if (Random.value > 0.5)
            {
                right = false;
                left = true;
            }

            initialPos = myPlayer.transform.position;
            pos = initialPos;
            firing = true;
        }
    }
}
