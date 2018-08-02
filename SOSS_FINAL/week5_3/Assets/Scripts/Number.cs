using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Number : MonoBehaviour {
    public static int num1 = 0;
    public static int num2 = 0;
    public TextMeshPro countText1;
    public TextMeshPro countText2;

    // Use this for initialization
    void Start () {

        TextMeshPro countText1 = GetComponent<TextMeshPro>();
        TextMeshPro countText2 = GetComponent<TextMeshPro>();

    }
	
	// Update is called once per frame
	void Update () {
        SetCountText1();
        SetCountText2();
    }
    void SetCountText1()
    {
        countText1.SetText("Count:" + num1.ToString());
    }

    void SetCountText2()
    {
        countText2.SetText("Count:" + num2.ToString());
    }
}
