using UnityEngine;
using UnityEngine.UI;

public class IPHAchievements : MonoBehaviour
{
    public AchievementsDatabase achievementdb;
    public GameObject achievementPrefab;
    public Transform contentParent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < achievementdb.achievements.Count; i++)
        {
            GameObject achievement = (GameObject)Instantiate(achievementPrefab, Vector2.zero, Quaternion.identity, contentParent);
            var _transform = achievement.transform;
            _transform.Find("Name").GetComponent<Text>().text = $"{achievementdb.achievements[i].name}";
            _transform.Find("Objective").GetComponent<Text>().text = $"{achievementdb.achievements[i].objective}";

            var _slider = achievement.transform.Find("ProgressSlider");
            _slider.Find("ProgressText").GetComponent<Text>().text = $"{achievementdb.achievements[i].currentGoal}/{achievementdb.achievements[i].finalGoal}";
            _slider.GetComponent<Slider>().value = achievementdb.achievements[i].progress;
        }
    }
}
