using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IPHStatistics : MonoBehaviour 
{
	// Use this for initialization
	void Start() 
	{
        // Set the statistics values in the statistics canvas
        GameObject.Find("TextDistance").GetComponent<Text>().text = "LONGEST DISTANCE: ??";
		GameObject.Find("TextStreak").GetComponent<Text>().text = "LONGEST STREAK: ??";
        GameObject.Find("TextTokens").GetComponent<Text>().text = "TOTAL FEATHERS: ??";
        GameObject.Find("TextPowerups").GetComponent<Text>().text = "TOTAL POWERUPS: ??";
        GameObject.Find("TextPowerupStreak").GetComponent<Text>().text = "LONGEST POWERUP: ??";
        GameObject.Find("TextCharacters").GetComponent<Text>().text = "CHARACTERS UNLOCKED: ??";
    }

}
