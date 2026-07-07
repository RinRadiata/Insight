using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    public TMP_Text titleText;
    public TMP_Text descText;
    public TMP_Text progressText;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh(QuestData quest)
    {
        titleText.text = quest.title;
        descText.text = quest.description;
        progressText.text = quest.currentCount + "/" + quest.targetCount;
    }
}