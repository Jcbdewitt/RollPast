using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickScript : MonoBehaviour
{
    public bool clicked = false;
    public bool pressed = false;
    public bool noText = false;

    Text thisButtonText;

    public string textToPlace;

    private void Start()
    {
        thisButtonText = this.GetComponentInChildren<Text>();

        UpdateText(textToPlace);
    }

    public void Button_Clicked()
    {
        clicked = true;
    }

    public void Button_Pressed()
    {
        pressed = true;
    }

    public void Button_Unpressed()
    {
        pressed = false;
    }

    public void UpdateText(string textToPut)
    {
        if (!noText)
        {
            textToPlace = textToPut;
            thisButtonText.text = textToPut;
        }
    }
    public void UpdateText()
    {

    }
}
