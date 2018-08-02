using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class number1 : MonoBehaviour {

    public static int num1 = 0;
    private TextMeshProUGUI countText1;

    // Use this for initialization
    void Start()
    {

        countText1 = GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        SetCountText1();
    }
    void SetCountText1()
    {
        countText1.SetText("Surviver:" + num1.ToString());
    }
}
