using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnnerScript : MonoBehaviour
{
    public GameObject PowerUp;

    public Texture[] PowerUpTextures;

    // Start is called before the first frame update
    void Start()
    {
        int typeOfPowerPoint = UnityEngine.Random.Range(0, PowerUpTextures.Length);
        GameObject spawnPoint = transform.GetChild(0).gameObject;

        GameObject newPowerUp = Instantiate(PowerUp, spawnPoint.transform.position, PowerUp.transform.rotation);
        newPowerUp.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", PowerUpTextures[typeOfPowerPoint]);
        newPowerUp.transform.GetChild(1).GetComponent<PowerUpColliderScript>().typeOfPowerUp = typeOfPowerPoint;
    }
}
