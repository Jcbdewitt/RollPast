using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigScript : MonoBehaviour
{
    public BoardSpawner boardSpawner;

    public AudioSource audioSource;

    public AudioClip loseSong;

    public bool onMenu = false;
    public bool playing = false;
    public bool muted = false;
    public bool moveCameraRig = true;
    public float speedFactor = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (moveCameraRig)
        {
            transform.Translate(Vector3.up * speedFactor * Time.deltaTime);
        }

        if (playing && !muted)
        {
            if (!onMenu)
            {
                PitchBasedOffSpeed();
            }

            if (audioSource.volume < 1f)
            {
                audioSource.volume += 0.05f;
            }
        } else if (muted)
        {
            audioSource.volume = 0f;
        }
    }

    private void PitchBasedOffSpeed()
    {
        if (speedFactor > 30f)
        {
            audioSource.pitch = 1.15f;
        } else if (speedFactor > 28f)
        {
            audioSource.pitch = 1.1f;
        } else if (speedFactor > 27f)
        {
            audioSource.pitch = 1f;
        } else if (speedFactor > 26.5f)
        {
            audioSource.pitch = .9f;
        } else
        {
            audioSource.pitch = .8f;
        }
    }

    public void ChangeToLoseSong()
    {
        playing = false;
        audioSource.pitch = 1f;
        audioSource.volume = .8f;
        audioSource.Stop();
        audioSource.clip = loseSong;
        audioSource.Play();
    }
}
