using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButtonScript : MonoBehaviour
{
    public GameControllerScript gameController;

    ButtonClickScript buttonScript;
    Button button;

    public int state = 0;

    public bool musicButton = true;
    public bool checkImage = true;

    public Sprite[] buttonStates;

    private void Start()
    {
        buttonScript = transform.gameObject.GetComponent<ButtonClickScript>();
        button = transform.gameObject.GetComponent<Button>();

        if (musicButton)
        {
            if (!gameController.musicMuted)
            {
                state = 0;
            } else
            {
                state = 1;
            }
        } else
        {
            if (!gameController.effectsMuted)
            {
                state = 0;
            } else
            {
                state = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ButtonDetect();
        if (checkImage) { ImageCheck(); }
    }

    private void ImageCheck()
    {
        button.image.sprite = buttonStates[state];
        checkImage = false;
    }

    private void ButtonDetect()
    {
        if (buttonScript.clicked)
        {
            buttonScript.clicked = false;
            checkImage = true;

            if (state == 0)
            {
                state = 1;

                if (musicButton)
                {
                    gameController.musicMuted = !gameController.musicMuted;
                    gameController.musicStateChange = true;
                } else
                {
                    gameController.effectsMuted = !gameController.effectsMuted;
                    gameController.effectStateChange = true;
                }

            }
            else
            {
                state = 0;

                if (musicButton)
                {
                    gameController.musicMuted = !gameController.musicMuted;
                    gameController.musicStateChange = true;
                }else
                {
                    gameController.effectsMuted = !gameController.effectsMuted;
                    gameController.effectStateChange = true;
                }
            }
        }
    }
}
