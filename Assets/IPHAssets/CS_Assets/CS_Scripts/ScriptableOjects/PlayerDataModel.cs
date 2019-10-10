using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataModel
{
    public PlayerDataModel() { }
    public PlayerDataModel(int lastScore, int selectedCharacter)
    {
        this.lastScore = lastScore;
        this.selectedCharacter = selectedCharacter;
    }

    //*Player Stats
    public int lastScore = 0;

    //Which character is selected to play
    public int selectedCharacter = 0;

    //Number os feathers collected from all games
    public int feathersCollected = 0;

    //Default ammount of highscores to keep track of
    public int topScoresAmmount = 10;

    //Top 10 highscores
    public List<int> topScores = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    //*Som
    //Sound and FX volume
    public float volume = 0.5f;

    //May the game play FX Sounds ?
    public bool canPlayFX = true;

    //May the game play Music Sounds ?
    public bool canplayMusic = true;

    //Achievements
    //Progressao e status dos achievs
}