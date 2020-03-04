using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarRotateScript : MonoBehaviour
{
    private Rigidbody rigid;

    public GameObject LeftButton;
    public GameObject RightButton;

    ButtonClickScript leftButtonScript;
    ButtonClickScript rightButtonScript;

    public float turnAmount = 1.0f;

    public bool onMenu = false;
    public bool receiveInput = true;
    public bool nonFlippedControls = true;

    public float maxTorque = 1.0f;
    private float turn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();

        if (!onMenu)
        {
            leftButtonScript = LeftButton.GetComponent<ButtonClickScript>();
            rightButtonScript = RightButton.GetComponent<ButtonClickScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!onMenu && receiveInput)
        {
            DetermineTurn();
        }
    }

    private void FixedUpdate()
    {

        rigid.AddTorque(transform.forward * (maxTorque * turn), ForceMode.VelocityChange);
        
    }

    private void DetermineTurn()
    {

        if ((leftButtonScript.pressed && rightButtonScript.pressed) || (!leftButtonScript.pressed && !rightButtonScript.pressed))
        {
            turn = 0.0f;
        }
        else if (leftButtonScript.pressed && !rightButtonScript.pressed)
        {
            if (!nonFlippedControls)
            {
                turn = -turnAmount;
            } else
            {
                turn = turnAmount;
            }
            
        }
        else if (rightButtonScript.pressed && !leftButtonScript.pressed)
        {
            if (!nonFlippedControls)
            {
                turn = turnAmount;
            } else
            {
                turn = -turnAmount;
            }
        }

    }
}
