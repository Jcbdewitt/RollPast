using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetizedColliderScript : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CoinCollider" && transform.parent.GetComponentInChildren<BallScript>().magnetized)
        {
            other.transform.parent.GetComponentInChildren<CoinColliderScript>().Ball = this.gameObject;
            other.transform.parent.GetComponentInChildren<CoinColliderScript>().magnetized = true;
        }
    }

}
