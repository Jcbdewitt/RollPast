using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBarScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball") || other.tag.Equals("Bar")) 
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        } else if (!(other.tag.Equals("MagneticCollider")))
        {
            Destroy(other.transform.parent.gameObject);
        }
    }

}
