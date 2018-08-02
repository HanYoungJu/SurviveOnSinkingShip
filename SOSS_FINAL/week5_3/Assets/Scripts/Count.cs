using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Count : MonoBehaviour {

    private TextMeshProUGUI uiText;
    // Use this for initialization

    public static float timer;
    public static bool canCount = true;
    public static bool doOnce = false;

    void Start()
    {
        timer = 35;
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uiText.SetText(timer.ToString("F"));
        }

        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            //uiText.SetText("0.00");
            timer = 0.0f;
            GameOver();
        }
    }
    void GameOver()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().end();
        StartCoroutine(restartGame());
    }
    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(3f);
        NetworkManager.instance.GetComponent<NetworkManager>().CommandRestart();
    }
}
