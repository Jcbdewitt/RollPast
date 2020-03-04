using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    enum positions { ONBAR, INAIR, HITBAR, HITWALL } 

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public AudioClip Hit;
    public AudioClip CoinCollect;
    public AudioClip powerUp;
    public AudioClip powerDown;
    public AudioClip lost;

    public GameObject SnapPoint;

    Rigidbody rigid;

    Animator animator;

    public float rollingSpeed = 0.0f;
    public float lastRotation = 0.0f;

    public bool magnetized = false;
    public bool gameEnded = false;
    public bool playingRoll = false;
    public bool playRollingSound = true;
    public bool playCrashSound = true;
    public bool muted = false;
    bool onlyOnce = true;
    bool ballDropped = false;
    bool animationDone = false;

    private void Start()
    {
        rigid = GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        IsGameOver();

        if (!muted)
        {
            AudioVolumes();
        } else
        {
            audioSource1.volume = 0f;
        }
    }

    private void AudioVolumes()
    {
        float rollingSpeed = rigid.angularVelocity.magnitude;

        if (rollingSpeed > 10f)
        {
            audioSource1.volume = .2f;
        }
        else if (rollingSpeed > 5f)
        {
            audioSource1.volume = .11f;
        }
        else if (rollingSpeed > 3f)
        {
            audioSource1.volume = .05f;
        }
        else
        {
            audioSource1.volume = 0f;
        }
    }

    private void IsGameOver()
    {
        if (gameEnded)
        {

            if (onlyOnce)
            {
                onlyOnce = false;
                audioSource2.PlayOneShot(lost, 15f);
                rigid.useGravity = false;
                rigid.isKinematic = true;
                transform.parent.transform.position = SnapPoint.transform.position;
                animator.SetBool("Sink Ball", true);
            }

            if (!onlyOnce && !animationDone && animator.GetCurrentAnimatorStateInfo(0).IsName("Ball Sunk Animation"))
            {
                animationDone = true;
                animator.SetBool("Sink Ball", false);
            }

            if (!ballDropped && animationDone && animator.GetCurrentAnimatorStateInfo(0).IsName("Ball Drop"))
            {
                ballDropped = true;
                transform.parent.transform.position = transform.position;
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                rigid.isKinematic = false;
                rigid.useGravity = true;
            }
        }
    }

    public void PlaySound(bool Enter)
    {
        if (Enter && playRollingSound && !muted)
        {
            if (!playingRoll)
            {
                playingRoll = true;
                audioSource1.Play();
            }
        } else
        {
            playingRoll = false;
            audioSource1.Stop();
        }
    }

    public void PlaySound(float impactForce)
    {
        if (playCrashSound && !muted)
        {
            if (impactForce > 70f)
            {
                audioSource2.PlayOneShot(Hit, .4f);
            } else if (impactForce > 30f)
            {
                audioSource2.PlayOneShot(Hit, .2f);
            } else 
            {
                audioSource2.PlayOneShot(Hit, .05f);
            }
            
            StartCoroutine(CrashSoundTimer(.5f));
        }
    }

    public void CoinSound()
    {
        if (!muted)
        {
            audioSource2.PlayOneShot(CoinCollect, 5f);
        }
    }

    public void PowerSound(bool up)
    {
        if (!muted)
        {
            if (up)
            {
                audioSource2.PlayOneShot(powerUp);
            }
            else
            {
                audioSource2.PlayOneShot(powerDown);
            }
        }
    }

    private IEnumerator CrashSoundTimer(float waitTime)
    {
        playCrashSound = false;
        yield return new WaitForSeconds(waitTime);
        playCrashSound = true;
    }
}
