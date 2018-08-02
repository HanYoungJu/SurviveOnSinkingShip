using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour {

    private float x;
    private float y;
    private float z;
    private int color;

    private void Start()
    {if (transform.position.z < 84.22f)
        {
            color = -1;
            number1.num1++;
        }
        else
        {
            color = 1;
            number2.num2++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 17.2f)
        {
            if (color == -1)
            {
                number1.num1--;
                color = 0;
            }
            if (color == 1)
            {
                number2.num2--;
                color = 0;
            }

        }
        else
        {
            if (transform.position.z <= 84.22f)
            {
                if (color == 0)
                {
                    number1.num1++;
                    color = -1;
                }
            }
            else if (84.22f < transform.position.z && transform.position.z <= 105f)
            {
                if (color == -1)
                {
                    number1.num1--;
                    color = 0;
                }
                else if (color == 1)
                {
                    number2.num2--;
                    color = 0;
                }
            }
            else if (105f < transform.position.z)
            {
                if (color == 0)
                {
                    number2.num2++;
                    color = 1;
                }
            }

        }


    }
    public int getColor()
    {
        return color;
    }
}
