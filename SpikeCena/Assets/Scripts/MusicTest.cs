using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTest : MonoBehaviour {

    public GameObject objectPrefab;
    public GameObject objectPrefab2;
    public GameObject backgroundPrefab;
    public float spawnThreshold = 0.5f;
    public int frequency;
    public FFTWindow fftWindow;
    public bool dampening;
    public float debugValue;
    public float lerpRate;
    public float lerpCap;
    public float colorLerpRate;
    public float colorScale;
    public float value;
    public float scaleThreshold;
    public float VO_constant;
    int number; //TODO check on breaking down into fewer components + damping
    int numberDrawn;
    float reverseScale;
    float xScale = .07f;
    private float[] samples;
    private float[] oldSamples;
    float sum;

    //VISUALIZTION
    GameObject[] _sampleCube;
    GameObject[] _antisampleCube;
    public float _VO_maxScale;
    public float _baseScale;
    public float _sumScale;
    public bool useTemp;
    public float VO_tempScale;
    public float mRed, mGreen, mBlue = 0f;
    public float m2Red, m2Green, m2Blue = 0f;

    GameObject bg;
    // Use this for initialization



    void Start () {
        number = 1024;
        numberDrawn = number / 2;
        reverseScale = number / 256;
        xScale /= reverseScale;
        //xScale /= 2; //remove overlap
        _sampleCube = new GameObject[number];
        _antisampleCube = new GameObject[number];
        samples = new float[number];
        oldSamples = new float[number];

        for (int i = 0; i < numberDrawn; i++)
        {
            GameObject cube = (GameObject)Instantiate(objectPrefab);
            cube.transform.position = new Vector3(xScale*i - 8.88f, -4.8f, 1f);
            cube.transform.localScale = new Vector3(xScale, .0001f, 1);
            cube.name = "sampleCube" + i;
            _sampleCube[i] = cube;

            GameObject anticube = (GameObject)Instantiate(objectPrefab);
            anticube.transform.position = new Vector3(xScale * -i + 8.88f, -4.8f, 1f);
            anticube.transform.localScale = new Vector3(xScale, .0001f, 1);
            anticube.name = "sampleCube" + i;
            _antisampleCube[i] = anticube;

        }
    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetSpectrumData(samples, 0, fftWindow);

        sum = 0;
        float midSection = 0;
        float mostImportant = numberDrawn; //NOTE: OLD WAS 128
        for (int i = 0; i < mostImportant; i++)
        {
            sum += samples[i]/2;
            if (i > 32)
            {
                midSection += samples[i]*4;
            }
        }

        VO_tempScale = _sumScale;
        VO_tempScale = _sumScale / (1 + sum*8);
        if (useTemp)
        {
            _VO_maxScale = _baseScale + VO_tempScale * sum;
        }
        else
        {
            _VO_maxScale = _baseScale + _sumScale * sum;
        }
        //_VO_maxScale = 2000 + sum*500;

        //How fast to lerp between points?
        float march4change = 10;
        lerpRate = .3f / (sum*march4change);
        float dampeningLerpRate = .3f / (sum*march4change);
        if (dampening == false)
        {
            if (lerpRate > lerpCap)
            {
                lerpRate = lerpCap;
            }
            else if (lerpRate < .05f) //we must have hit a REALLY intense part - lets ACTUALLY speed it up!
            {
                lerpRate = .08f;
            }
        }
        else
        {
            if (lerpRate > lerpCap)
            {
                lerpRate = lerpCap;
            }
            else if (lerpRate < .1f) //we must have hit a REALLY intense part - lets ACTUALLY speed it up!
            {
                lerpRate = .1f;
            }
        }
        
        value = sum / (sum + 1); //approaches 1
        if (colorScale != 0)
        {
            value /= colorScale; //at most can be .5
        }
        if (dampening)
        {
            value *= .7f;
        }
        changeBackgroundColor(value);
        for (int i = 0; i < numberDrawn; i++)
        {
            if (_sampleCube != null)
            {
                Color newcolor;
                if (dampening == false)
                {
                    Vector2 startScale = _sampleCube[i].transform.localScale;
                    Vector2 endScale = new Vector2(xScale, ((samples[i] / 1.0f) * _VO_maxScale) + 1); //TODO scale down 
                    _sampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                    _antisampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                    newcolor = new Color(mRed + value, mGreen - value, mBlue - value);
                }
                else //############# DAMPENING!!! #############
                {
                    newcolor = new Color(m2Red - value / 2, m2Green + value / 2, m2Blue + value / 2);
                    Vector2 startScale = _sampleCube[i].transform.localScale;
                    if (samples[i] * _VO_maxScale + 1 < startScale.y)
                    {
                        if (oldSamples[i] <= 0) //maybe hasn't been init yet!
                        {
                            oldSamples[i] = .01f;// / 8; //.01f;
                        }
                        else
                        {
                            VO_constant = 1; // 1.25f;
                            //float speedModification = (midSection / (midSection + 1));
                            float speedModification = (sum / (sum + 1));
                            speedModification /= 4; //at max can be .25
                            if (speedModification > .25f)
                            {
                                speedModification = .25f;
                            }
                            VO_constant += speedModification;
                            //VO_constant = 1.25f; //TODO make a function similar to check if variance enough / enough time to change!
                            oldSamples[i] *= ((VO_constant /* + lerpRate*/) + Time.deltaTime); //TODO cap this too? NOTE: ## lerping here
                        }
                        float xScale = startScale.x;
                        float yScale = startScale.y - oldSamples[i]; //-  a growing scalar
                        Vector2 vector = new Vector2(xScale, yScale);
                        _sampleCube[i].transform.localScale = vector;
                        _antisampleCube[i].transform.localScale = vector;

                    }
                    else
                    {
                        oldSamples[i] = .01f; // dampeningLerpRate / 4;// / 8; //.01f;
                        //@@@@@@SCALING BARS DOWN
                        Vector2 endScale = new Vector2(xScale, ((samples[i] / 1.0f) * _VO_maxScale) + 1); //TODO scale down 
                        _sampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                        _antisampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                    }
                    
                }
                Color oldcolor = _sampleCube[i].GetComponent<Renderer>().material.color; //they are mirrored so we only need this
                Color audioColor = Color.Lerp(oldcolor, newcolor, colorLerpRate);
                _sampleCube[i].GetComponent<Renderer>().material.color = audioColor;
                _antisampleCube[i].GetComponent<Renderer>().material.color = audioColor;
            }
        }
	}

    void changeBackgroundColor(float value)
    {
        Color oldC = Camera.main.backgroundColor;
        Color newC;
        if (dampening == false)
        {
            newC = new Color(mRed - value, .2f - value, .35f - value);
        }
        else
        {
            value *= .5f;
            newC = new Color(m2Red + value, .2f + value, .35f + value);
        }
        Color currentColour = Color.Lerp(oldC, newC, 0.025f);
        Camera.main.backgroundColor = currentColour;
    }

    public float getIntensity()
    {
        return sum;
    }
}


