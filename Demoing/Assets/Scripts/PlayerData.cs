using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int money = 0;
    public int[] currentEquipped = { 0, 0, 0 };
    public bool rotate = true;
    public bool musicMuted = false;
    public bool effectMuted = false;

    public PlayerData()
    {

    }

    public PlayerData(GameControllerScript gameControllerScript)
    {
        money = gameControllerScript.finalScoreValue;
        currentEquipped = gameControllerScript.currentEquipped;
    }

    public PlayerData(GameControllerScript gameControllerScript, PlayerData playerData)
    {
        if (gameControllerScript.purchaseMade)
        {
            money = gameControllerScript.money;
            currentEquipped = gameControllerScript.currentEquipped;
        }
        else if (!gameControllerScript.resetingEverything)
        {
            this.money = playerData.money + gameControllerScript.finalScoreValue;
            currentEquipped = gameControllerScript.currentEquipped;
        } else
        {
            this.money = 0;
        }

        this.rotate = gameControllerScript.rotate;
        this.musicMuted = gameControllerScript.musicMuted;
        this.effectMuted = gameControllerScript.effectsMuted;

    }
}
