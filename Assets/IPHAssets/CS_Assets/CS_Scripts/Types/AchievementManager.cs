using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public static List<Achievement> achievements;
    private static string dataPath;

    //Awake for singleton pattern
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log($" {this.name} - Creating new Singleton instance");
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log($" {this.name} - Destroying instances other than the original.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        // ! Windows -> Application.persistentDataPath = C:\Users\burca\AppData\LocalLow\Joysticket\Jungle Jump
        dataPath = Path.Combine(Application.persistentDataPath, "Achievements.json");
        achievements = LoadAchievements();
    }

    //Reset the persistence data to default values
    public static void ResetProfile()
    {
        achievements = new List<Achievement>();
        SaveAchievements();
    }

    public static string SaveAchievements()
    {
        return SaveAchievements(achievements);
    }

    //Save player data to json file at path and return the written string
    public static string SaveAchievements(List<Achievement> data)
    {
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }

        return jsonString;
    }

    //Load player data from json at path and return a PlayerData object
    public static List<Achievement> LoadAchievements()
    {
        if (!File.Exists(dataPath))
        {
            SaveAchievements(new List<Achievement>());
        }

        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<List<Achievement>>(jsonString);
        }
    }

    //Load player data from json at path and return a string
    public static string LoadAchievementsString()
    {
        if (!File.Exists(dataPath))
        {
            SaveAchievements(new List<Achievement>());
        }

        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            return jsonString;
        }
    }
}