using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playmenu : MonoBehaviour {

    public GameObject play;
    public GameObject pause;
    static bool menu;
    


    void Start()
    {
        menu = true;
    }

    // Use this for initialization
    void Update()
    {
        if (menu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu = false;
                play.SetActive(false);
                pause.SetActive(true);

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu = true;
                pause.SetActive(false);
                play.SetActive(true);
            }

        }
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
