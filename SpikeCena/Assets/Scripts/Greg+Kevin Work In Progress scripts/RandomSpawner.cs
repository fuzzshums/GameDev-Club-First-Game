using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : BurstSpawner {

    public bool repeating;
    public float duration; //time in seconds

    private Stack<GameObject> disabledGO;

    //Sets Burst Random Spawner
    /// <summary>
    /// Sets the random spawner to burst mode
    /// </summary>
    /// <param name="m">max number of spikes to spawn</param>
    /// <param name="si">interval between spawns in seconds</param>
    public RandomSpawner(int m, float si = 0.2f) : base(m, si)
    {
        spawnInterval = si;
        repeating = false;
        duration = 0f;
    }

    //Sets Continuous Random Spawner
    /// <summary>
    /// More options to set for Random Spawner
    /// </summary>
    /// <param name="m">max number of spikes to spawn</param>
    /// <param name="r">does spawner repeat</param>
    /// <param name="d">duration of spawn in seconds</param>
    /// <param name="si">interval between spawns in seconds</param>
    public RandomSpawner(int m, bool r, float d, float si = 0.2f) : base(m, si)
    {
        repeating = r;
        duration = d;
    }

    void Start()
    {
        Initiate();
        //repeatedly spawn spikes
        if (repeating)
        {
            StartCoroutine(MyUpdate(transform.position));
        }
        else
        {
            StartCoroutine(base.MyUpdate(transform.position));
        }
    }

    protected override void Initiate()
    {
        base.Initiate();
        disabledGO = new Stack<GameObject>();
    }

    protected override void Spawn(GameObject go, Vector3 spawnPos)
    {
        //if max amount not spawned yet, spawn
        if (spawned.Count < max)
        {
            //randomly generate location of spike
            float xPos = Random.Range(-7f, 5f);
            float yPos = Random.Range(4f, 6f);
            Vector3 newPos = new Vector3(xPos, yPos);

            //Instantiate and add spike to spawned list
            GameObject copy = Instantiate(go, newPos, Quaternion.identity, transform);
            spawned.Add(copy);
            copy.SetActive(true);
            numToDisable++;
        }
        //---------code for repeating spawn------------------
        else if (disabledGO.Count > 0)
        {
            //randomly generate location of spike
            float xPos = Random.Range(-7f, 5f);
            float yPos = Random.Range(4f, 6f);
            Vector3 newPos = new Vector3(xPos, yPos);

            //respawn disabled GO
            GameObject disabled = disabledGO.Pop();
            int index = spawned.IndexOf(disabled);
            spawned[index].transform.position = newPos;
            spawned[index].SetActive(true);
            numToDisable++;
        }
    }

    public override void Despawn(GameObject go)
    {
        base.Despawn(go);
        disabledGO.Push(go);
    }

    public override IEnumerator MyUpdate(Vector3 spawnPos)
    {
        float timer = 0f;
        //repeating spawn behavior
        while (timer < duration)
        {
            Spawn(spawnPos);
            timer = timer + Time.deltaTime + spawnInterval;
            yield return new WaitForSeconds(spawnInterval);

        }
        //wait until all spawned GO are disabled before disabling the spawner
        while (allDisabled == false)
        {
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.SetActive(false);
    }
}
