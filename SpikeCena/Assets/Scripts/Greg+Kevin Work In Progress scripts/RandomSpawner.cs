using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : BurstSpawner {

    public float spawnInterval;
    public bool repeating;
    public float duration; //time in seconds

    private Stack<GameObject> disabledGO;

    void Start()
    {
        Initiate();
        //repeatedly spawn spikes
        if (repeating)
        {
            StartCoroutine(MyUpdate(transform.position));
        }
        //else create a random burst of spikes
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
        //spawn while duration has not expired
        while (timer < duration)
        {
            Spawn(spawnPos);
            timer = timer + Time.deltaTime + spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }
        //wait until all spawned GO are disabled before disabling the spawner
        while (allDisabled == false)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
