﻿using UnityEngine;
using UnityEngine.UI;

public class IPHStatistics : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        PlayerData.PlayerStats playerStats = PersistenceController.playerData.playerStats;

        //Refatoracao: Mudando strings para interpolação
        // Set the statistics values in the statistics canvas
        GameObject.Find("TextDistance").GetComponent<Text>().text = $"LONGEST DISTANCE: {playerStats.topScores[0]}";
        GameObject.Find("TextStreak").GetComponent<Text>().text = $"LONGEST STREAK: {playerStats.longestStreak}";
        GameObject.Find("TextTokens").GetComponent<Text>().text = $"TOTAL FEATHERS: {playerStats.feathersCollected}";
        GameObject.Find("TextPowerups").GetComponent<Text>().text = $"TOTAL POWERUPS: {playerStats.powerupsCollected}";
        //GameObject.Find("TextPowerupStreak").GetComponent<Text>().text = $"LONGEST POWERUP: {}"; // ?
        GameObject.Find("TextCharacters").GetComponent<Text>().text = $"CHARACTERS UNLOCKED: {playerStats.charactersUnlocked}";
    }

}
