using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpColliderScript : MonoBehaviour
{
    public int typeOfPowerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            GameObject gameController = GameObject.Find("GameController");

            GameControllerScript gameControllerScript = gameController.GetComponent<GameControllerScript>();

            gameControllerScript.PowerUp(typeOfPowerUp);

            Destroy(transform.parent.gameObject);
        }
        else if (other.tag == "Bar")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
