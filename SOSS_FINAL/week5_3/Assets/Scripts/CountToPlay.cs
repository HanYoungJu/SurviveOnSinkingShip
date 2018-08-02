using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountToPlay : MonoBehaviour {

    public GameObject CountDown;
    public GameObject Main;

	// Use this for initialization
	void Start () {
        StartCoroutine(Delay());
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3.5f);
        CountDown.SetActive(false);
        Main.SetActive(true);
    }
}
