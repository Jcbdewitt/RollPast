using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpinScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * 50);
    }
}
