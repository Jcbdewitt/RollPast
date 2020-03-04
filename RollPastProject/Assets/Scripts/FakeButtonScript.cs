using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeButtonScript : MonoBehaviour
{
    private byte unpressedRGB = 255;
    private byte pressedRGB = 200;
    public void Update()
    {
        if (transform.parent.GetComponent<ButtonClickScript>().pressed)
        {
            GetComponent<Image>().color = new Color32(pressedRGB, pressedRGB, pressedRGB, 255);
        } else
        {
            GetComponent<Image>().color = new Color32(unpressedRGB, unpressedRGB, unpressedRGB, 255);
        }
    }
}
