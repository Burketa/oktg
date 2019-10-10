using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class PlayerData : MonoBehaviour
{
    private static PlayerDataModel playerData;
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

        playerData.lastScore = 0;
        playerData.selectedCharacter = 0;
        playerData.feathersCollected = 0;
        playerData.topScoresAmmount = 10;

        playerData.topScores = new List<int>();
        for (int i = 0; i < playerData.topScoresAmmount; i++)
            playerData.topScores.Add(0);

        playerData.volume = 0.5f;
        playerData.canplayMusic = true;
        playerData.canPlayFX = true;

        SavePlayerData(playerData, dataPath);
    }

    //Put the highscore in the list, if it is indeed a highscore and trim the list to topScoresAmmount size
    public static void CheckHighscore(int possibleHighscore)
    {
        int index = 0;

        foreach (int score in new List<int>(playerData.topScores))
        {
            if (possibleHighscore >= score)
            {
                playerData.topScores.Insert(index, possibleHighscore);
                playerData.topScores = playerData.topScores.GetRange(0, playerData.topScoresAmmount);
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
/*


using System.IO;
using UnityEngine;

public class JsonCharacterSaver : MonoBehaviour
{
    public CharacterData characterData;
    string dataPath;

    void Start ()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "CharacterData.txt");
    }

    void Update ()
    {
        if(Input.GetKeyDown (KeyCode.S))
            SaveCharacter (characterData, dataPath);

        if (Input.GetKeyDown (KeyCode.L))
            characterData = LoadCharacter (dataPath);
    }

    static void SaveCharacter (CharacterData data, string path)
    {
        string jsonString = JsonUtility.ToJson (data);

        using (StreamWriter streamWriter = File.CreateText (path))
        {
            streamWriter.Write (jsonString);
        }
    }

    static CharacterData LoadCharacter (string path)
    {
        using (StreamReader streamReader = File.OpenText (path))
        {
            string jsonString = streamReader.ReadToEnd ();
            return JsonUtility.FromJson<CharacterData> (jsonString);
        }
    }
}


 */
