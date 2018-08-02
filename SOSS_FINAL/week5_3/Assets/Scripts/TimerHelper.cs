using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerHelper : MonoBehaviour {

    Image timerBar;
    TimerScript T;
    public GameObject timesUpText;
    public GameObject RespawnText;
    public float maxTime = 5f;
    float timeLeft;

    // Use this for initialization
    void Start () {
        timesUpText.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }
	
	// Update is called once per frame
	void Update () {

        /*if (T.Reset)
        {
            RespawnText.SetActive(true);
            timesUpText.SetActive(false);
            timeLeft = maxTime;
            T.Reset = false;
        }*/
		
	}
}
