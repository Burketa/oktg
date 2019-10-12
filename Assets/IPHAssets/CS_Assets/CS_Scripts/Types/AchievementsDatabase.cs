using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee;

[CreateAssetMenu(fileName = "AchievementsDatabase", menuName = "Achievements Database")]
public class AchievementsDatabase : ScriptableObject
{

    [SerializeField, Reorderable(paginate = true, pageSize = 5, elementNameProperty = "achievements.id")]
    public AchievementsArrayList achievements;

    [System.Serializable]
    public class AchievementsArrayList : ReorderableArray<Achievement>
    {
    }
}
