using UnityEngine;
using TMPro;
using System.IO;

public class SaveSlotUI : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text photoText;
    public TMP_Text questText;

    int slotIndex;
    string path;

    public void Setup(int index)
    {
        slotIndex = index;
        path = Application.persistentDataPath + "/save_" + index + ".json";

        Refresh();
    }

    public void Refresh()
    {
        if (!File.Exists(path))
        {
            timeText.text = "Empty Slot";
            photoText.text = "";
            questText.text = "";
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        timeText.text = data.saveTime;
        photoText.text = "Photos: " + data.photoCount;

        int completed = 0;
        int total = 0;

        if (data.quests != null)
        {
            total = data.quests.Length;

            foreach (var q in data.quests)
            {
                if (q.completed)
                    completed++;
            }
        }

        questText.text = $"Quest: {completed}/{total}";
    }

    public void Save()
    {
        SaveManager.Instance.SaveGame(slotIndex);
        Refresh();

        FindAnyObjectByType<SaveLoadUI>()?.Refresh(); // update the whole save/load UI slots
    }

    public void Load()
    {
        SaveManager.Instance.LoadGame(slotIndex);
    }

    public void Delete()
    {
        if (File.Exists(path))
            File.Delete(path);

        Refresh();
    }
}