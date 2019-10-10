using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IPHStatistics : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //PlayerData data = new PlayerData();

        // Set the statistics values in the statistics canvas
        GameObject.Find("TextDistance").GetComponent<Text>().text = "LONGEST DISTANCE: " + PlayerData.playerData.playerStats.topScores[0];
        GameObject.Find("TextStreak").GetComponent<Text>().text = "LONGEST STREAK: " + PlayerData.playerData.playerStats.longestStreak;
        GameObject.Find("TextTokens").GetComponent<Text>().text = "TOTAL FEATHERS: " + PlayerData.playerData.playerStats.feathersCollected;
        GameObject.Find("TextPowerups").GetComponent<Text>().text = "TOTAL POWERUPS: " + PlayerData.playerData.playerStats.powerupsCollected;
        GameObject.Find("TextPowerupStreak").GetComponent<Text>().text = "LONGEST POWERUP: ??"; // ?
        GameObject.Find("TextCharacters").GetComponent<Text>().text = "CHARACTERS UNLOCKED: " + PlayerData.playerData.playerStats.charactersUnlocked;
    }

}
