using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats
{
    private static int score, numHits, numSpawned, maxSpikeSpawn, totalSpikesSpawned, numPowCollected;
    private static float hitRatio;
    private static bool songEnded;

    public static bool SongEnded
    {
        get
        {
            return songEnded;
        }
        set
        {
            songEnded = value;
        }
    }

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    public static int NumHits
    {
        get
        {
            return numHits;
        }
        set
        {
            numHits = value;
        }
    }

    public static int NumSpawned
    {
        get
        {
            return numSpawned;
        }
        set
        {
            numSpawned = value;
        }
    }

    public static float HitRatio
    {
        get
        {
            return hitRatio;
        }
        set
        {
            hitRatio = value;
        }
    }

    public static int MaxSpikeSpawn
    {
        get
        {
            return maxSpikeSpawn;
        }
        set
        {
            maxSpikeSpawn = value;
        }
    }

    public static int TotalSpikesSpawned
    {
        get
        {
            return totalSpikesSpawned;
        }
        set
        {
            totalSpikesSpawned = value;
        }
    }

    public static int NumPowCollected
    {
        get
        {
            return numPowCollected;
        }
        set
        {
            numPowCollected = value;
        }
    }

}