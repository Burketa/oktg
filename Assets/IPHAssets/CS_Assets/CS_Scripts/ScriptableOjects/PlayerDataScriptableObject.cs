using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataScriptableObject")]
public class PlayerDataScriptableObject : ScriptableObject
{
    //Player Stats
    //public int playerStats;
    //Score from last game session
    public int lastScore;
    //Which character is selected to play
    public int selectedCharacter;
    //Number os feathers collected from all games
    public int feathersCollected;
    //Top 10 highscores
    public int[] top10Scores;

    //Som
    //Sound and FX volume
    public float volume;
    //May the game play FX Sounds ?
    public bool canPlayFX;
    //May the game play Music Sounds ?
    public bool canplayMusic;

    //Achievements
    //Progressao e status dos achievs
}

//?fazer com que sejam ox X highscores ?
//?Colocar com Regions ?
