using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColliderScript : MonoBehaviour
{

    public BallScript Ball;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 15f)
        {
            if (collision.gameObject.tag == "Column")
            {
                Ball.PlaySound(collision.relativeVelocity.magnitude);
            }

            if (collision.gameObject.tag == "Bar")
            {
                Ball.PlaySound(true);
                Ball.PlaySound(collision.relativeVelocity.magnitude);
            }
        } else
        {
            if (collision.gameObject.tag == "Bar")
            {
                Ball.PlaySound(true);
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Bar")
        {
            Ball.PlaySound(false);
        }
    }

}
