using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class PersistenceController : MonoBehaviour
{
    public static PersistenceController instance;
    public static PlayerData playerData;
    private static string dataPath;

    //Awake for singleton pattern
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Creating new Singleton instance");
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Destroying instances other than the original.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        // ! Windows -> Application.persistentDataPath = C:\Users\burca\AppData\LocalLow\Joysticket\Jungle Jump
        dataPath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        playerData = LoadPlayerData();
    }

    //Reset the persistence data to default values
    public static void ResetProfile()
    {
        playerData = new PlayerData();
        SavePlayerData();
    }

    //Put the highscore in the list, if it is indeed a highscore and trim the list to topScoresAmmount size
    public static bool CheckHighscore(int possibleHighscore)
    {
        int index = 0;

        foreach (int score in new List<int>(playerData.playerStats.topScores))
        {
            if (possibleHighscore >= score)
            {
                playerData.playerStats.topScores.Insert(index, possibleHighscore);
                playerData.playerStats.topScores = playerData.playerStats.topScores.GetRange(0, playerData.playerStats.topScoresAmmount);
                SavePlayerData();
                return true;
            }
            index++;
        }
        return false;
    }

    public static string SavePlayerData()
    {
        return SavePlayerData(playerData);
    }

    //Save player data to json file at path and return the written string
    public static string SavePlayerData(PlayerData data)
    {
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }

        return jsonString;
    }

    //Load player data from json at path and return a PlayerData object
    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(dataPath))
        {
            SavePlayerData(new PlayerData());
        }

        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<PlayerData>(jsonString);
        }
    }

    //Load player data from json at path and return a string
    public static string LoadPlayerDataString()
    {
        if (!File.Exists(dataPath))
        {
            SavePlayerData(new PlayerData());
        }

        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            return jsonString;
        }
    }
}