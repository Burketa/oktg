using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public Achievements achievements = new Achievements();
    public PlayerStats playerStats = new PlayerStats();
    public SoundConfig soundConfig = new SoundConfig();

    //Constructor
    public PlayerData()
    {
        playerStats.lastScore = 0;
        playerStats.feathersCollected = 0;
        playerStats.charactersUnlocked = 0;
        playerStats.selectedCharacter = 0;
        playerStats.longestStreak = 0;
        playerStats.powerupsCollected = 0;
        playerStats.topScoresAmmount = 10;

        playerStats.topScores = new List<int>();
        for (int i = 0; i < playerStats.topScoresAmmount; i++)
            playerStats.topScores.Add(0);

        soundConfig.canPlayFX = true;
        soundConfig.canplayMusic = true;
        soundConfig.volume = 0.5f;
    }

    //PlayerStats inner class
    [Serializable]
    public class PlayerStats
    {
        //Score from last game session
        public int lastScore = 0;

        //Longest streak of jumps
        public int longestStreak = 0;

        //Total number of powerups collected from all games
        public int powerupsCollected = 0;

        //Number of characters the player already unlocked
        public int charactersUnlocked = 0;

        //Which character is selected to play
        public int selectedCharacter = 0;

        //Number os feathers collected from all games
        public int feathersCollected = 0;

        //Default ammount of highscores to keep track of
        public int topScoresAmmount = 10;

        //Top 10 highscores
        public List<int> topScores = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    }

    //SoundConfig inner class
    [Serializable]
    public class SoundConfig
    {
        //Sound and FX volume
        public float volume = 0.5f;

        //May the game play FX Sounds ?
        public bool canPlayFX = true;

        //May the game play Music Sounds ?
        public bool canplayMusic = true;

    }
    [Serializable]
    public class Achievements
    {
        //TODO: Achievements !
        public float volume = 0.5f;

    }
}