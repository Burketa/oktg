using UnityEngine;
using UnityEngine.UI;

public class IPHHighscores : MonoBehaviour
{
    public GameObject highScorePrefab;

    public Transform contentParent;

    // Start is called before the first frame update
    void Start()
    {
        PlayerDataModel.PlayerStats playerStats = PlayerData.playerData.playerStats;

        for (int i = 0; i < playerStats.topScoresAmmount; i++)
        {
            GameObject score = (GameObject)Instantiate(highScorePrefab, Vector2.zero, Quaternion.identity, contentParent);
            score.GetComponent<Text>().text = $"{i + 1} - {playerStats.topScores[i]}";
        }
    }
}
