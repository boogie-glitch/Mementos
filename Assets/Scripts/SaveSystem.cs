using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void NewGame()
    {
        PlayerData playerData = new PlayerData(1, 0, 100, 100, 100, "Level1");
        SavePlayer(playerData);
        Debug.Log("New game started and player data saved.");
    }
    public static void SavePlayer(PlayerData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
