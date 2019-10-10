public static class PlayerData
{
    public static PlayerDataScriptableObject playerData;

    //Reset the persistency data to default values
    public static void ResetProfile()
    {
        playerData.lastScore = 0;
        playerData.selectedCharacter = 0;
        playerData.feathersCollected = 0;

        for (int i = 0; i < 10; i++)
            playerData.top10Scores[i] = 0;

        playerData.volume = 0.5f;
        playerData.canplayMusic = false;
        playerData.canPlayFX = false;
    }
}
