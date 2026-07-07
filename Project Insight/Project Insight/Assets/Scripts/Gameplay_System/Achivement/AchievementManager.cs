using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void CheckAchievements()
    {
        int photoCount = PhotoManager.Instance.photos.Count;

        if (photoCount == 1)
        {
            Debug.Log("Achievement unlocked: First Memo");

            AchievementUI.Instance.Show(
                "Achievement Unlocked!",
                "First Memo"
            );
        }

        if (photoCount == 5)
        {
            Debug.Log("Achievement unlocked: Memorizer");

            AchievementUI.Instance.Show(
                "Achievement Unlocked!",
                "Memorizer"
            );
        }
    }
}