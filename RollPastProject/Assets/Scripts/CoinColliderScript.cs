using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinColliderScript : MonoBehaviour
{
    public GameObject coinParent;
    public GameObject Ball;

    public bool magnetized = false;
    bool onlyOnce = true;

    private Vector3 lastPosition;

    private Vector3 startPosition;

    private Vector3 endPosition;

    private float lerpTime = 3;
    private float currentLerpTime = 0;

    public void Awake()
    {
        coinParent = transform.parent.gameObject;
    }

    public void Update()
    {
        if (magnetized)
        {
            if (onlyOnce)
            {
                startPosition = coinParent.transform.position;
            }

            endPosition = Ball.transform.position;
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            coinParent.transform.position = Vector3.Lerp(startPosition, endPosition, perc);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            GameObject gameController = GameObject.Find("GameController");

            GameControllerScript gameControllerScript = gameController.GetComponent<GameControllerScript>();

            gameControllerScript.coinsCollected++;

            other.gameObject.GetComponentInChildren<BallScript>().CoinSound();

            Destroy(transform.parent.gameObject);
        } else if (other.tag == "Bar")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
