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
    public float bandLerpRate;
    public float bandLerpCap;
    public float scaleThreshold;

    int number = 1024; //TODO check on breaking down into fewer components + damping
    float reverseScale;
    float xScale = .07f;
    private float[] samples;
    private float[] oldSamples;
    private float[] bands;
    private float[] oldBands;
    private Vector2[] bandStartPos;
    float sum;

    //VISUALIZTION
    GameObject[] _sampleCube;
    GameObject[] _antisampleCube;
    GameObject[] _bands;
    GameObject[] _antibands;
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
        reverseScale = number / 256;
        xScale /= reverseScale;
        //xScale /= 2; //remove overlap
        _sampleCube = new GameObject[number];
        _antisampleCube = new GameObject[number];
        samples = new float[number];
        oldSamples = new float[number];

        //instantiate background
        //bg = Instantiate(backgroundPrefab);


        for (int i = 0; i < number; i++)
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

        bands = new float[8];
        oldBands = new float[8];
        bandStartPos = new Vector2[8];
        _bands = new GameObject[0];
        _antibands = new GameObject[0];

        for (int j = 0; j < _bands.Length; j++)
        {
            GameObject band = (GameObject)Instantiate(objectPrefab2);
            band.transform.localScale = new Vector3(.05f, .0001f, 1);
            band.transform.position = new Vector3(1.8f*j-6,-1.0f,10);
            band.name = "BAND" + j;
            _bands[j] = band;
            /*
            GameObject antiband = (GameObject)Instantiate(objectPrefab2);
            antiband.transform.localScale = new Vector3(.05f, .0001f, 1);
            antiband.transform.position = new Vector3(1.8f * -j + 6, -2.4f, 10);
            antiband.name = "ANTIBAND" + j;
            _antibands[j] = antiband;
            */
        }


    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetSpectrumData(samples, 0, fftWindow);

        sum = 0;
        float mostImportant = number; //NOTE: OLD WAS 128
        for (int i = 0; i < mostImportant; i++)
        {
            sum += samples[i]/2;
        }
        float sliceLength = samples.Length / bands.Length;
        float pausePoint = sliceLength;
        int intervalPoint = 0;
        //filling in bands

        clearBands();
        //oldBands = bands;
        float maximum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            if (i < pausePoint)
            {
                bands[intervalPoint] += samples[i]; //adding range into a band //OPTION 1
                if (maximum < samples[i])
                {
                    maximum = samples[i];
                }

            }
            else
            {
                //bands[intervalPoint] = maximum;  // OPTION 2 
                maximum = 0;
                intervalPoint += 1;
                pausePoint += sliceLength;
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
        for (int i = 0; i < number; i++)
        {
            if (_sampleCube != null)
            {
                if (dampening == false)
                {
                    //_sampleCube[i].transform.localScale = 
                    Vector2 startScale = _sampleCube[i].transform.localScale;
                    Vector2 endScale = new Vector2(xScale, ((samples[i] / 1.0f) * _VO_maxScale) + 1); //TODO scale down 
                    _sampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                    _antisampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);

                    //color = newcolor;
                }
                else //############# DAMPENING!!! #############
                {
                    Vector2 startScale = _sampleCube[i].transform.localScale;
                    if (samples[i] * _VO_maxScale + 1 < startScale.y)
                    {
                        if (oldSamples[i] <= 0) //maybe hasn't been init yet!
                        {
                            oldSamples[i] = .01f;// / 8; //.01f;
                        }
                        else
                        {
                            oldSamples[i] *= ((1.25f /* + lerpRate*/) + Time.deltaTime); //TODO cap this too? NOTE: ## lerping here
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
                        if (samples[i] > scaleThreshold)
                        {
                            //samples[i] /= 4;
                        }
                        Vector2 endScale = new Vector2(xScale, ((samples[i] / 1.0f) * _VO_maxScale) + 1); //TODO scale down 
                        _sampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                        _antisampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                    }
                }
                Color newcolor;
                if (dampening == false)
                {
                    newcolor = new Color(mRed + value, mGreen - value, mBlue - value);
                }
                else
                {
                    newcolor = new Color(m2Red - value/2, m2Green + value/2, m2Blue + value/2);
                }
                Color oldcolor = _sampleCube[i].GetComponent<Renderer>().material.color; //they are mirrored so we only need this
                Color audioColor = Color.Lerp(oldcolor, newcolor, colorLerpRate);

                _sampleCube[i].GetComponent<Renderer>().material.color = audioColor;
                _antisampleCube[i].GetComponent<Renderer>().material.color = audioColor;

            }
        }
        // @@@@@@@@@@@@@@@@@@@@@@@@ MODIFY BANDS HERE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        float constant = .15f;
        float mConstant = 1.5f;
        for (int i = 0; i  < _bands.Length; i++)
        {
            Vector2 startBandScale = _bands[i].transform.localScale;
            if ((bands[i] * mConstant + constant) < _bands[i].transform.localScale.y) //DAMPENING (if new less than old!)
            {
                if (oldBands[i] < bandLerpCap)
                {
                    oldBands[i] *=  (1.25f + Time.deltaTime); //wtf even is this
                }
                float xScale = startBandScale.x - oldBands[i];
                float yScale = startBandScale.y - oldBands[i]; //-  a growing scalar
                _bands[i].transform.localScale = new Vector2(xScale, yScale);
                //_antibands[i].transform.localScale = new Vector2(xScale, yScale);
                
            }
            else
            {
                Vector2 endBandScale;
                oldBands[i] = .01f;
                endBandScale = new Vector2((bands[i] * mConstant) + constant, (bands[i] * mConstant) + constant);
                _bands[i].transform.localScale = Vector2.Lerp(startBandScale, endBandScale, bandLerpRate);
               // _antibands[i].transform.localScale = Vector2.Lerp(startBandScale, endBandScale, bandLerpRate);
            }
            /*
            float xPos = _bands[i].transform.position.x;
            float yPos = _bands[i].transform.position.y - oldBands[i];
            if (yPos < bandStartPos[i].y)
            {
                _bands[i].transform.position = bandStartPos[i];
            }

            _bands[i].transform.position = new Vector3(xPos, yPos, 10);
            */
            /*
            if (i == 0) //eye case
            {
                if (_bands[i].transform.position.y < 3 - .5f * i)
                {
                    Vector3 newPos = new Vector3(_bands[i].transform.position.x, _bands[i].transform.position.y + bands[i], 10);
                    Vector3 antiNewPos = new Vector3(_antibands[i].transform.position.x, _antibands[i].transform.position.y + bands[i], 10);

                    _bands[i].transform.position = Vector3.Lerp(_bands[i].transform.position, newPos, lerpRate);
                    _antibands[i].transform.position = Vector3.Lerp(_antibands[i].transform.position, antiNewPos, lerpRate);
                }
            }
            else
            {
                if (_bands[i].transform.position.y < -1 + .05f*i) // 0 - .5f*i
                {
                    Vector3 newPos = new Vector3(_bands[i].transform.position.x, _bands[i].transform.position.y + bands[i], 10);
                    Vector3 antiNewPos = new Vector3(_antibands[i].transform.position.x, _antibands[i].transform.position.y + bands[i], 10);

                    _bands[i].transform.position = Vector3.Lerp(_bands[i].transform.position, newPos, lerpRate);
                    _antibands[i].transform.position = Vector3.Lerp(_antibands[i].transform.position, antiNewPos, lerpRate);
                }
            }
            */

            if (!dampening)
            {
                _bands[i].GetComponent<Renderer>().material.color = new Color(.3f, .1f, .1f);
                //_antibands[i].GetComponent<Renderer>().material.color = new Color(.3f, .1f, .1f);
            }
            else
            {
                _bands[i].GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                //_antibands[i].GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
            }

        }
	}

    
    void clearBands()
    {
        for (int i = 0; i < _bands.Length; i++)
        {
            bands[i] = 0;
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


