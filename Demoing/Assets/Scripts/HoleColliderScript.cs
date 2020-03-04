using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleColliderScript : MonoBehaviour
{
    public GameObject connectedCollider;

    public HoleColliderScript[] connectedColliders;

    public GameObject GameControllerObject;

    public GameObject SnapPoint;

    public bool connected = false;
    public bool noHole = false;
    public bool countsOnce = true;

    private void Start()
    {
        SnapPoint = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameControllerScript gameControllerScript = GameControllerObject.GetComponent<GameControllerScript>();

        if (other.tag == "Ball" && countsOnce && !connected)
        {
            countsOnce = false;
            if (noHole)
            {
                if (!gameControllerScript.gameEnded)
                {
                    gameControllerScript.rowsPassed += 1;
                }
            } else
            {
                if (!gameControllerScript.boosting)
                {
                    gameControllerScript.SnapPoint = SnapPoint;
                    gameControllerScript.gameEnded = true;
                    foreach (HoleColliderScript s in connectedColliders)
                    {
                        s.countsOnce = false;
                    }
                } else
                {
                    gameControllerScript.rowsPassed += 1;
                }
                
            }
        }

        if (other.tag == "Ball" && connected)
        {
            Debug.Log("Touch connect collider");
            if (connectedCollider.GetComponent<HoleColliderScript>().countsOnce)
            {
                Debug.Log("in if statement");
                countsOnce = false;
                connectedCollider.GetComponent<HoleColliderScript>().countsOnce = false;

                if (!gameControllerScript.gameEnded)
                {
                    if (!gameControllerScript.boosting)
                    {
                        gameControllerScript.SnapPoint = SnapPoint;
                        gameControllerScript.gameEnded = true;
                       
                    }
                    else
                    {
                        gameControllerScript.rowsPassed += 1;
                    }
                }

            }
        }
    }

}
