using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descText;
    public TMP_Text progressText;

    public void Setup(QuestData quest)
    {
        titleText.text = quest.title;
        descText.text = quest.description;

        progressText.text = quest.currentCount + "/" + quest.targetCount;

        if (quest.completed)
            progressText.color = Color.green;
    }
}