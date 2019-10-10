﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class PlayerData : MonoBehaviour
{
    public static PlayerDataModel playerData;
    private static string dataPath;

    public void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        playerData = LoadPlayerData(dataPath);
    }

    //Reset the persistence data to default values
    public static void ResetProfile()
    {
        playerData = new PlayerDataModel();

        playerData.playerStats.lastScore = 0;
        playerData.playerStats.longestStreak = 0;
        playerData.playerStats.powerupsCollected = 0;
        playerData.playerStats.charactersUnlocked = 0;
        playerData.playerStats.selectedCharacter = 0;
        playerData.playerStats.feathersCollected = 0;
        playerData.playerStats.topScoresAmmount = 10;

        playerData.playerStats.topScores = new List<int>();
        for (int i = 0; i < playerData.playerStats.topScoresAmmount; i++)
            playerData.playerStats.topScores.Add(0);

        playerData.soundConfig.volume = 0.5f;
        playerData.soundConfig.canplayMusic = true;
        playerData.soundConfig.canPlayFX = true;

        SavePlayerData(playerData, dataPath);
    }

    //Put the highscore in the list, if it is indeed a highscore and trim the list to topScoresAmmount size
    public static void CheckHighscore(int possibleHighscore)
    {
        int index = 0;

        foreach (int score in new List<int>(playerData.playerStats.topScores))
        {
            if (possibleHighscore >= score)
            {
                playerData.playerStats.topScores.Insert(index, possibleHighscore);
                playerData.playerStats.topScores = playerData.playerStats.topScores.GetRange(0, playerData.playerStats.topScoresAmmount);
                SavePlayerData(playerData, dataPath);
                break;
            }
            index++;
        }
    }

    public static void SavePlayerData(PlayerDataModel data, string path)
    {
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static PlayerDataModel LoadPlayerData(string path)
    {
        if (!File.Exists(path))
        {
            SavePlayerData(new PlayerDataModel(), path);
        }

        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<PlayerDataModel>(jsonString);
        }
    }
}