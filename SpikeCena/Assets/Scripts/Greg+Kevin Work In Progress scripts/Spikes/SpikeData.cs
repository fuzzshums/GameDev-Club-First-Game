using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* The purpose of the SpikeData class is to separate the data
* from GUI/gameobject components.
* 
* It also allows different types of spikes to share the same properties
* yet, different implementations.
* 
*/
public class SpikeData
{

    //Variables
    private int m_damage;          //damage spike causes
    private int m_health;          //health of spike
    private float m_xSpeed;        //x velocity
    private float m_ySpeed;        //y velocity

    //Default Constructor
    public SpikeData()
    {
        m_damage = 5;
        m_health = 10;
        m_xSpeed = 0f;
        m_ySpeed = 0f;
    }

    //Getter and Setter Properties
    public int damage { get { return m_damage; } set { m_damage = value; } }
    public int health { get { return m_health; } set { m_health = value; } }
    public float xSpeed { get { return m_xSpeed; } set { m_xSpeed = value; } }
    public float ySpeed { get { return m_ySpeed; } set { m_ySpeed = value; } }

}

