using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.photoCount = PhotoManager.Instance.photos.Count;
        data.saveTime = System.DateTime.Now.ToString();

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }
}