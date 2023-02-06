using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SerializationManager
{
    static string dir = Application.persistentDataPath + "/saves/"; // Directory to save in
    
    //The saveData is given as SaveData.Current
    public static bool SaveGame(string saveName, object saveData) 
    {
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        string json = JsonUtility.ToJson(saveData, true); // Converts data saved to json

        File.WriteAllText(dir + saveName + ".save", json); // Saves to disk

        return true;
    }

    public static bool LoadGame(string saveName)
    {
        if(!File.Exists(dir + saveName)) return false; // Failed Load Check

        SaveData temp = new SaveData();

        string data = File.ReadAllText(dir + saveName); // Reading the json file
        temp = JsonUtility.FromJson<SaveData>(data); // Converting from json to SaveData

        SaveData.Current = temp; // Updating Data

        return true;
    }

}
