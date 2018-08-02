using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class number2 : MonoBehaviour {

    public static int num2 = 0;
    private TextMeshProUGUI countText2;

    // Use this for initialization
    void Start()
    {
        countText2 = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCountText2();
    }
    void SetCountText2()
    {
        countText2.SetText("Surviver:" + num2.ToString());
    }
}
