using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OutOfBounds : MonoBehaviour
{
    private static Dictionary<Spike, Spawner> spikeDict;

    private void Awake()
    {
        spikeDict = new Dictionary<Spike, Spawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("spike"))
        {
            Spawner spawner = null;
            bool spikeExists = spikeDict.TryGetValue(other.GetComponent<Spike>(), out spawner);
            if (spikeExists == false)
            {
                spikeDict.Add(other.GetComponent<Spike>(), other.GetComponentInParent<Spawner>());
                spikeDict.TryGetValue(other.GetComponent<Spike>(), out spawner);
            }
            spawner.Despawn(other.gameObject);
        }

        //TODO: Add other tag checks
    }
}
