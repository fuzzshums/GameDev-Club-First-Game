using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* Spawner that stops spawning once max amount is reached.
*   - Good for spawning small number of things total
*   - Can be memory heavy if spawn many objects total b/c does not reuse when enabled
*   
* Specifics
*   - Instantiates objects at location of spawner until max is reached.
*   - Pools objects as they are despawned.
*   - When all objects are despawned, spawner disables self.
*   - When spawner reenabled, it spawns the despawned objects.
*/
public class BurstSpawner : Spawner
{
    protected bool allDisabled;

    void Start()
    {
        Initiate();
        StartCoroutine(MyUpdate(transform.position));
    }

    protected override void Initiate()
    {
        base.Initiate();

        GameObject copy = Instantiate(defaultGO, transform.position, Quaternion.identity, transform);
        defaultGO.SetActive(true);
        copy.SetActive(true);

        spawned.Add(copy);

        allDisabled = false;
    }

    protected override void Spawn(GameObject go, Vector3 spawnPos)
    {
        //if max amount not spawned yet, spawn
        if (spawned.Count < max)
        {
            GameObject copy = Instantiate(go, spawnPos, Quaternion.identity, transform);
            spawned.Add(copy);
            copy.SetActive(true);
            numToDisable++;
        }
    }

    public override void Despawn(GameObject go)
    {
        base.Despawn(go);
        if(numToDisable == 0)
        {
            allDisabled = true;
        }
    }

    public override IEnumerator MyUpdate(Vector3 spawnPos)
    {
        //spawn while spawned < max
        while (spawned.Count < max)
        {
            Spawn(spawnPos);
            //yield return new WaitForSeconds(interval);
            yield return null;
        }
        //wait until all spawned GO are disabled before disabling the spawner
        while (spawned.Count == max && allDisabled == false)
        {
            yield return null;
            //yield return new WaitForSeconds(interval);
        }
        gameObject.SetActive(false);
    }
    /*
        //Gets the index of a GameObject in spawned if it shares the same type (script)
        //as go and is disabled
        private int GetIndexOfSameTypeAndDisabled(GameObject go)
        {
            if (tags.Contains(go.tag))
            {
                //SPIKE CHECK
                if (go.tag == "spike")
                {
                    //Checks if targetComponent equals one in spawned
                    //If true and GO is disabled, return the index
                    Spike targetComponent = go.GetComponent<Spike>();
                    for (int i = 0; i < spawned.Count; i++)
                    {
                        Spike itemComponent = spawned[i].GetComponent<Spike>();
                        string tcString = targetComponent.ToString().Split(null)[1];
                        string icString = targetComponent.ToString().Split(null)[1];
                        if (itemComponent != null && tcString == icString)
                        {
                            if (spawned[i].activeSelf == false)
                            {
                                return i;
                            }
                        }
                    }
                }
                //TODO: Add more tag checks if needed
            }
            return -1;
        }

        /// <summary>
        /// Spawns a GameObject at a specified location in World Space.
        /// </summary>
        /// <param name="go"> Game Object to spawn </param>
        /// <param name="spawnPos"> Position to spawn Game Object at </param>
        public override void Spawn(GameObject go, Vector3 spawnPos)
        {
            //if there are objects to disable, this means some are disabled and some are enabled
            if (numToDisable >= 0 || numToDisable <= max)
            {
                //if object is already in pool and disabled
                //enable and spawn it
                int index = GetIndexOfSameTypeAndDisabled(go);
                if (index != -1)
                {
                    spawned[index].SetActive(true);
                    spawned[index].transform.position = spawnPos;
                    numToDisable++;
                }
            }
            //if there are no disabled objects or GO is not in spawned
            //and count < max, add to spawned and spawn it
            if (spawned.Count < max)
            {
                GameObject copy = Instantiate(go, spawnPos, Quaternion.identity, transform);
                spawned.Add(copy);
                copy.SetActive(true);
                numToDisable++;
            }
            //else if count == max, can't spawn
        }
 */
}
