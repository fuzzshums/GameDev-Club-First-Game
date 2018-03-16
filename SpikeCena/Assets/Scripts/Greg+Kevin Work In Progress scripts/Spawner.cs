using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* Spawns any type of GameObject 
* and acts as an object pool
*/
public abstract class Spawner: MonoBehaviour
{
    //protected variables
    protected List<GameObject> spawned;
    protected int numToDisable;

    //private variables
    private bool initiated = false;

    //public variables
    public GameObject defaultGO;
    public int max;
    public float interval;

    //public static variables
    public static List<string> tags;

    //initiates static, protected, and private vars
    protected virtual void Initiate()
    {
        if(initiated == false)
        {
            tags = new List<string>();
            tags.Add("spike");
            //TODO: add more tags
            // ...
            // ..
            // .
            initiated = true;
        }

        spawned = new List<GameObject>();
        numToDisable = 0;
    }

    /// <summary>
    /// Spawns a GameObject at a specified location in World Space.
    /// </summary>
    /// <param name="go"> Game Object to spawn </param>
    /// <param name="spawnPos"> Position to spawn Game Object at </param>
    protected virtual void Spawn(GameObject go, Vector3 spawnPos)
    {

    }

    public abstract IEnumerator MyUpdate(Vector3 spawnPos);

    //Spawns that default GO at a position
    protected void Spawn(Vector3 spawnPos)
    {
        Spawn(defaultGO, spawnPos);
    }

    ///Sets GO to inactive
    public virtual void Despawn(GameObject go)
    {
        go.transform.position = transform.position;
        go.SetActive(false);
        numToDisable--;
    }

    //Get if tags initiated
    public bool isInitiated()
    {
        return initiated;
    }

    //Changes default GO
    public GameObject GetDefault()
    {
        return defaultGO;
    }

    //Changes default GO
    public void SetDefault(GameObject go)
    {
        defaultGO = go;
    }
}
