using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTest : MonoBehaviour {

    public GameObject objectPrefab;
    public float spawnThreshold = 0.5f;
    public int frequency;
    public FFTWindow fftWindow;
    public float debugValue;
    public float lerpRate;
    public float colorScale;
    public float value;

    int number = 512;
    float reverseScale;
    float xScale = .07f;
    private float[] samples;
    float sum;

    //VISUALIZTION
    GameObject[] _sampleCube;
    GameObject[] _antisampleCube;
    public float _maxScale;
    public float mRed, mGreen, mBlue = 0f;
    // Use this for initialization
    void Start () {
        reverseScale = number / 256;
        xScale /= reverseScale;
        //xScale /= 2; //remove overlap
        _sampleCube = new GameObject[number];
        _antisampleCube = new GameObject[number];
        samples = new float[number];
        for (int i = 0; i < number; i++)
        {
            GameObject cube = (GameObject)Instantiate(objectPrefab);
            cube.transform.position = new Vector3(xScale*i - 8.85f, -4.8f, 1f);
            cube.name = "sampleCube" + i;
            _sampleCube[i] = cube;

            GameObject anticube = (GameObject)Instantiate(objectPrefab);
            anticube.transform.position = new Vector3(xScale * -i + 8.85f, -4.8f, 1f);
            anticube.name = "sampleCube" + i;
            _antisampleCube[i] = anticube;

        }
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetSpectrumData(samples, 0, fftWindow);
        sum = 0;
        float mostImportant = 128;
        for (int i = 0; i < mostImportant; i++)
        {
            sum += samples[i];
        }
        _maxScale = 1000 + sum*3000;
        
        lerpRate = .3f / (sum*8);
        if (lerpRate > .3f)
        {
            lerpRate = .3f;
        } else if (lerpRate < .05f) //we must have hit a REALLY intense part - lets ACTUALLY speed it up!
        {
            lerpRate = .1f;
        }
        
        value = sum / (sum + 1);
        if (colorScale != 0)
        {
            value /= colorScale; //at most can be .5
        }
        
        for(int i = 0; i < number; i++)
        {
            if (_sampleCube != null)
            {
                //_sampleCube[i].transform.localScale = 
                Vector2 startScale = _sampleCube[i].transform.localScale;
                Vector2 endScale = new Vector2(xScale, (samples[i] * _maxScale) + 1);
                _sampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);
                _antisampleCube[i].transform.localScale = Vector2.Lerp(startScale, endScale, lerpRate);

                Color oldcolor = _sampleCube[i].GetComponent<Renderer>().material.color;
                Color newcolor = new Color(mRed + value, mGreen - value, mBlue - value, 1F);
                _sampleCube[i].GetComponent<Renderer>().material.color = Color.LerpUnclamped(oldcolor, newcolor, Mathf.PingPong(Time.time, 1));
                _antisampleCube[i].GetComponent<Renderer>().material.color =  Color.LerpUnclamped(oldcolor, newcolor, Mathf.PingPong(Time.time, 1));
                //color = newcolor;
            }
        }
	}
}
