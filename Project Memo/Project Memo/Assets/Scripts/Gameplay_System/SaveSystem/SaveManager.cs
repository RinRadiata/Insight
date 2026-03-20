using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    void Awake()
    {
        Instance = this;
    }

    string GetPath(int slot)
    {
        return Application.persistentDataPath + "/save_" + slot + ".json";
    }

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();

        data.photoCount = PhotoManager.Instance.photos.Count;

        var questList = QuestManager.Instance.quests;

        data.quests = new QuestSaveData[questList.Count];

        for (int i = 0; i < questList.Count; i++)
        {
            data.quests[i] = new QuestSaveData()
            {
                id = questList[i].id,
                currentCount = questList[i].currentCount,
                targetCount = questList[i].targetCount,
                completed = questList[i].completed
            };
        }

        data.saveTime = System.DateTime.Now.ToString("MMM dd, yyyy HH:mm");

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(GetPath(slot), json);
    }

    public void LoadGame(int slot)
    {
        string path = GetPath(slot);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save data");
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        var questList = QuestManager.Instance.quests;

        foreach (var savedQuest in data.quests)
        {
            var quest = questList.Find(q => q.id == savedQuest.id);

            if (quest != null)
            {
                quest.currentCount = savedQuest.currentCount;
                quest.targetCount = savedQuest.targetCount;
                quest.completed = savedQuest.completed;
            }
        }

        //refresh ui
        QuestListUI questUI = FindFirstObjectByType<QuestListUI>();
        if (questUI != null)
            questUI.Show(questList);

        PhotoManager.Instance.LoadPhotosFromDisk();

        FindFirstObjectByType<PhotoPageManager>()?.Refresh();

        Debug.Log("Loaded slot " + slot);
    }
}