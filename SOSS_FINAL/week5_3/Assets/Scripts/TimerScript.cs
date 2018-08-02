using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    Image timerBar;
    public float maxTime = 5f;
    float timeLeft;
    public GameObject timesUpText;
    public GameObject RespawnText;
    public GameObject Respawn;
    public GameObject Main;
    //public bool Reset = false;


    // Use this for initialization
    void Start () {
        timesUpText.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            RespawnText.SetActive(false);
            timesUpText.SetActive(true);
            timerBar.enabled=false;
            timeLeft = 5f;
        }
	}

    private void LateUpdate()
    {
        RespawnText.SetActive(true);
        timesUpText.SetActive(false);
        timerBar.enabled = true;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
    }
}
