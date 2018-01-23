using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Manages the spawning of all GameObjects
* There should only be one per scene
*/
public class SpawnManager : MonoBehaviour {

    public List<GameObject> spikes;     //list of spike prefabs
    public List<GameObject> bullets;    //list of bullet prefabs

    public List<Transform> spawnPoints; //list of spawn points

    private List<Vector3> spawnPos;      //list of spawn point positions
    //private Spawner spikeSpawner;
    public List<Spawner> spawners;     //list of different types of spawners
                                                //Note: A list of spawners might be excessive
                                                //      One spawner may be enough to handle both 
                                                //      the spikes and bullets

    public static SpawnManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        //instantiate
        spawnPos = new List<Vector3>();

        //Set up spawn point postions and spawners
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnPos.Add(spawnPoint.position);
        }
        //StartCoroutine(spawners[0].MyUpdate(spawners[0].transform.position));
    }
	
	// Update is called once per frame
	void Update () {

    }
}
