using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnScript : MonoBehaviour
{
    public GameObject Coin;

    private int numberOfLocations;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLocations = transform.childCount;

        for (int i = 0; i < numberOfLocations; i++)
        {
            GameObject currentLocation = transform.GetChild(i).gameObject;

            Instantiate(Coin, currentLocation.transform.position, Coin.transform.rotation);
        }
    }
}
