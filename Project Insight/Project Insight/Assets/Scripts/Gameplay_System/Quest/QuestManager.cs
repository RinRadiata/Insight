using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestData> quests = new List<QuestData>();

    void Awake()
    {
        Instance = this;
    }

    public void AddProgress(int amount = 1)
    {
        foreach (var quest in quests)
        {
            if (quest.completed) continue;

            quest.currentCount += amount;

            if (quest.currentCount >= quest.targetCount)
            {
                quest.completed = true;

                Debug.Log("Quest Completed: " + quest.title);

                if (AchievementUI.Instance != null)
                {
                    AchievementUI.Instance.Show(
                        "Quest Completed",
                        quest.title
                    );
                }
            }
        }
    }
}