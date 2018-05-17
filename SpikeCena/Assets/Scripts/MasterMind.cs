using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class will manage all of the variables of everything else.
//It will have getters that every other class can call to find out info.
//Everything communicates through mastermind.

// Win condition @ end of song fix
// Music can be added by player - explore the asset
// Ui cleaned up
// finish the area of focus. - visual adjustments - maybe option to disable
// sprites?
// Particles  * could be impossible to add
// Credits! Options / pause difficulty!
// Option to chose visualization mode!
// TODO make powerups fall down!
// Add a WAY OUT!! HELP of the build!

public class MasterMind : MonoBehaviour {

    GameObject objectManager;
    GameObject musicManager;
    GameObject player;
    GameObject myAreaOfFocus;

    //View Only's
    static public float VO_rate;
    public float[] Intensity;
    //Use to modify params of other classes
    public float rateCap;
    public float playerRateCap;

    //Spikes
    float oldNumSpikes;
    float newNumSpikes;
    float numSpikeThreshold;
    float timeWithinThreshold;
    float timeCap;
    float totalTime;
    public static float areaOfFocusXScale;
    float timeWithinThresholdAOF;

    //public vars
    public int maxBullets; //max bullets player can store

    //Scoring
    public int damagePenalty;
    public int hitSpikePoints;
    public int powerupPoints;

    //private scripts
    private Player s_player;

    //UI
    public Text timeText;
    float time = 0;
    public Text scoreText;
    int score = 0;
    public Text healthText;
    int health = 0;
    public Text ammoText;
    int ammo = 0;

    //stats
    private int totalDamage;
    private int numHits; //number of spikes shot down
    private int numSpawned; //number of bullets spawned;
    private float hitRatio;
    private int maxSpikeSpawn; //max spikes on screen;
    private int totalSpikesSpawned;
    private int numPowCollected;

    //management
    static public bool game_over;
    static public float silence_timer;

    // Use this for initialization
    void Start () {
        objectManager = GameObject.Find("Object Manager");
		musicManager = GameObject.Find("Music Master");
        player = GameObject.Find("Player");
        myAreaOfFocus = GameObject.Find("Area Of Focus");
        s_player = player.GetComponent<Player>();
        Intensity = new float[2];

        //Variables
        rateCap = 12f;
        playerRateCap = 12f;

        numSpikeThreshold = 1f;
        timeWithinThreshold = 0;
        timeCap = .33f;
        totalTime = 0;

        timeText.text = "Seconds Alive: " + ((int)Time.deltaTime);
        scoreText.text = "Score: " + 0;

        //stat init
        health = s_player.health;
        numHits = 0;
        numSpawned = 0;
        hitRatio = 0f;
        maxSpikeSpawn = 0; 
        totalSpikesSpawned = 0;
        numPowCollected = 0;

        //management
        game_over = false;
        silence_timer = 0;
    }

    // Update is called once per frame
    void Update () {
        totalTime += Time.deltaTime;
        time = time + Time.deltaTime;
        timeText.text = "Seconds Alive: " + ((int)time).ToString();
        int s = (int)(score + time);
        scoreText.text = "Score: " + s.ToString();
        //healthText.text = "Health: " + displayAmount(health, false);
        //ammoText.text = "Ammo: " + displayAmount(ammo, true);
        calculateWhiteMovementRate();
        check_game_over();
        
    }

    void check_game_over()
    {
        float value = musicManager.GetComponent<MusicTest>().getIntensity();
        //Debug.Log(value);
        if (musicManager.GetComponent<MusicTest>().getIntensity() < .00002563369f)
        {
            silence_timer += Time.deltaTime;
            if (silence_timer > 10)
            {
                Debug.Log("GAME OVER!");
                game_over = true;
            }
        }
        else
        {
            silence_timer = 0;
            game_over = false;
        }
        
    }

    private void LateUpdate()
    {
        //set some stats
        if(numSpawned > 0)
        {
            hitRatio = ((float)numHits / numSpawned) * 100;
        }        
    }
    //1.) @@@@@@   SPIKES   @@@@@@
    #region
    private void calculateWhiteMovementRate()
    {
        float dF = 7f / 4f;
        float oldRate = VO_rate;
        float newRate = .5f + 4f * musicManager.GetComponent<MusicTest>().getIntensity(); //was 7
        if (newRate > rateCap)
        {
            newRate = rateCap;
        }
        if (newRate * 4f / dF < oldRate && newRate * 6f / dF >= oldRate)
        {
            Debug.Log("PLAYER mid slowing");
            VO_rate = Mathf.Lerp(oldRate, newRate, .2f);

        }
        else if (newRate * 6f / dF < oldRate)
        {
            Debug.Log("PLAYER GOD slowing");
            //myAreaOfFocus.GetComponent<Area_Of_Focus>().setChoice(UnityEngine.Random.Range(0, 3));
            VO_rate = Mathf.Lerp(oldRate, newRate, 1f);

        }
        else if (newRate < oldRate)
        {
            //Dampen the speed decrease!
            VO_rate = oldRate - (.25f / dF) * Time.deltaTime;
        }
        else //if its super LOW or greater than -> just lerp it
        {
            VO_rate = Mathf.Lerp(oldRate, newRate, .5f);
        }
    }
    public float getWhiteMovementRate()
    {
        return VO_rate;
    }
    public int getNumSpikes()
    {
        float Scale = totalTime / 100;
        if (Scale < 1)
        {
            Scale = 1;
        }
        else if (Scale > 2)
        {
            Scale = 2;
        }
        else
        {
            //Debug.Log(Scale);
        }
        newNumSpikes = 5 + Scale * 20f * musicManager.GetComponent<MusicTest>().getIntensity();
        float bassBoostCount = Scale * 40f * musicManager.GetComponent<MusicTest>().getBass();
        newNumSpikes += bassBoostCount; //add a weight for bass
        Intensity[0] = newNumSpikes;
        Intensity[1] = bassBoostCount;
        //TODO scale timewithinThreshold by current intensity -> i.e. "slow time when game slows time?"
        float check = timeWithinThreshold * (this.getWhiteMovementRate() /2);
        if (Mathf.Abs(newNumSpikes - oldNumSpikes) > numSpikeThreshold && check > timeCap) //check was timeWithinThreshold
        {
            timeWithinThreshold = 0;
            
            if (newNumSpikes > oldNumSpikes + 1)
            {
                oldNumSpikes += 1; //don't give away all our spikes at once!
                return (int)oldNumSpikes;
            }
            

            if (newNumSpikes + 2 < oldNumSpikes)
            {
                oldNumSpikes -= 2;
                return (int) oldNumSpikes;
            }
            else if (newNumSpikes < oldNumSpikes)
            {
                oldNumSpikes -= 1;
                return (int) oldNumSpikes;
            }
        }
        else
        {
            timeWithinThreshold += Time.deltaTime;
            newNumSpikes = oldNumSpikes;
        }
        oldNumSpikes = newNumSpikes;
        return (int) newNumSpikes;
    }
    public float getAreaOfFocusScale()
    {
        float previous = areaOfFocusXScale;
        float current = 3f + 4f * musicManager.GetComponent<MusicTest>().getIntensity();
        if (current > 6)
        {
            current = 8.88f*2; //lerp towards max!
            areaOfFocusXScale = Mathf.Lerp(previous, current, .1f); //lerp faster speed!
            return areaOfFocusXScale;
        }
        if (current < previous && previous > 6)
        {
            if (timeWithinThresholdAOF > 5f)
            {
                timeWithinThresholdAOF = 0;
            }
            else
            {
                timeWithinThresholdAOF += Time.deltaTime;
                return previous; //do nothing this time
            }
            
        }
        
        areaOfFocusXScale = Mathf.Lerp(previous, current, .01f);
        return areaOfFocusXScale;
    }
    #endregion
    //2.) @@@@@@   PLAYER   @@@@@@
    #region
    public void modifyHealth(int n)
    {
        health += n;
        healthText.text = "Health: " + displayAmount(health, false);
    }
    #endregion
    //3.) @@@@@@     UI     @@@@@@
    #region
    private string displayAmount(int n, bool k)
    {
        if (!k && n > 10)
        {
            return " a lot";
        }
        string amount = "";
        if (n <= maxBullets)
        {
            if (n <= 10)
            {
                for (int i = 0; i < n; i++)
                {
                    amount = amount + "|";
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    amount = amount + "|";
                }
                amount = amount + " x" + (n - 10);
            }
        }
        /*
        for (int i = 0; i < n; i++)
        {
            if (i % 10 == 0 && i >= 10 && k)
            {
                amount = amount + " x" + (n-10);
            }
            else
            {
                amount = amount + "|";
            }
        }
       */
        return amount;
    }
    public void increaseAmmo()
    {
        ammo += 10;
        ammoText.text = "Ammo: " + displayAmount(ammo, true);
    }
    public void decreaseAmmo()
    {
        ammo--;
        ammoText.text = "Ammo: " + displayAmount(ammo, true);
    }
    #endregion
    //4.) @@@@@@    STATS   @@@@@@
    #region
    public void modifyScore(int n)
    {
        score += n;
    }
    public void setMaxSpawned(int n)
    {
        if(maxSpikeSpawn < n)
        {
            maxSpikeSpawn = n;
        }        
    }
    public void increaseNumSpikeSpawned(int n)
    {
        totalSpikesSpawned += n;
    }
    public void increaseBulletSpawned(int n)
    {
        numSpawned += n;
    }
    public void increaseNumHits(int n)
    {
        numHits += n;
    }
    public void increasePowPickedUp()
    {
        numPowCollected++;
    }
    public void finalizeStats()
    {
        Stats.Score = score;
        Stats.MaxSpikeSpawn = maxSpikeSpawn;
        Stats.TotalSpikesSpawned = totalSpikesSpawned;
        Stats.NumSpawned = numSpawned;
        Stats.HitRatio = hitRatio;
        Stats.NumHits = numHits;
        Stats.NumPowCollected = numPowCollected;
    }

    #endregion
    //5.) @@@@@@ MANAGEMENT @@@@@@
    public bool get_game_over_check()
    {
        return game_over;
    }
    #region
    #endregion
}
