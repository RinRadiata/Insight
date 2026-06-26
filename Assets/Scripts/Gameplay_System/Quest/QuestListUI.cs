using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    public Transform content;
    public GameObject questItemPrefab;

    public void Show(List<QuestData> quests)
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (var quest in quests)
        {
            GameObject item = Instantiate(questItemPrefab, content);

            QuestItemUI ui = item.GetComponent<QuestItemUI>();
            ui.Setup(quest);
        }
    }
}