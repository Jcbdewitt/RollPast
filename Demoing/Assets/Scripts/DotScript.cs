using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotScript : MonoBehaviour
{
    public bool Highlight = false;

    public Sprite[] states;

    public void UpdateHighLight()
    {
        Image thisImage = GetComponent<Image>();

        if (!Highlight)
        {
            thisImage.sprite = states[0];
        }else
        {
            thisImage.sprite = states[1];
        }
    }
}
