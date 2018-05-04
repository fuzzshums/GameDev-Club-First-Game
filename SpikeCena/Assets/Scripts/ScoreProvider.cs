using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProvider : MonoBehaviour {

    public Text statsText;

    // Use this for initialization
    void Start () {
        statsText.text = "Score: " + Stats.Score + "\n" +
                         "Number of Hits: " + Stats.NumHits + "\n" +
                         "Bullets Spawned: " + Stats.NumSpawned + "\n" +
                         "Hit Ratio: " + Stats.HitRatio + "\n" +
                         "Max Spikes Spawned: " + Stats.MaxSpikeSpawn + "\n" +
                         "Total Spikes Spawned: " + Stats.TotalSpikesSpawned + "\n" +
                         "Powerups Collected: " + Stats.NumPowCollected + "\n";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
