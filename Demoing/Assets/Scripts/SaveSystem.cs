using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SavePlayer(GameControllerScript gameControllerScript)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.gat";
        FileStream stream;
        PlayerData data;

        if (File.Exists(path))
        {
            stream = new FileStream(path, FileMode.Open);

            PlayerData currentData = formatter.Deserialize(stream) as PlayerData;

            data = new PlayerData(gameControllerScript, currentData);

            stream.Close();

        } else
        {
            data = new PlayerData(gameControllerScript);
        }


        stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePurchase(PurchaseData purchaseData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/purchase.gat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, purchaseData);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerData.gat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            PlayerData data = new PlayerData();

            return data;
        }
    }

    public static PurchaseData LoadPurchase()
    {
        string path = Application.persistentDataPath + "/purchase.gat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PurchaseData data = formatter.Deserialize(stream) as PurchaseData;
            stream.Close();

            return data;
        }
        else
        {
            PurchaseData data = new PurchaseData();

            return data;
        }
    }
}
