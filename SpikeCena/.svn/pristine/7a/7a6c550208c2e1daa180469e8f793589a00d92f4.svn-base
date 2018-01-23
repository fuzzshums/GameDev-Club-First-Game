using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* The purpose of the Spike class is to set 
* requirements for future spikes to have.
* 
* These requirements are:
*   Move() - a movement method 
*   Die()  - disables spike and do something coolz maybe
*   Others...?
* 
*/
public abstract class Spike: MonoBehaviour {

    //data is hidden within spikeData
    private SpikeData spikeData;

    protected void Initiate()
    {
        spikeData = new SpikeData();
    }

    //Getters and Setters to limit access to data
    public int GetDamage()
    {
        return spikeData.damage;
    }

    public int GetHealth()
    {
        return spikeData.health;
    }

    public void SetHealth(int health)
    {
        spikeData.health = health;
    }

    public float GetXSpeed()
    {
        return spikeData.xSpeed;
    }

    public float GetYSpeed()
    {
        return spikeData.ySpeed;
    }

    public abstract void Move();

    public abstract void Die();

}
