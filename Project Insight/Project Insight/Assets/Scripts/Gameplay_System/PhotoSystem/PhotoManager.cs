using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance;

    public List<PhotoData> photos = new List<PhotoData>();

    string folderPath;

    void Awake()
    {
        Instance = this;

        folderPath = Application.persistentDataPath + "/Photos/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    public PhotoData AddPhoto(Texture2D image, PhotoTarget target)
    {
        PhotoData data = new PhotoData();

        // make sure it unique file name 
        string fileName = "photo_" + System.DateTime.Now.Ticks + ".png";
        string fullPath = folderPath + fileName;

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

        data.filePath = fullPath;
        data.image = image;
        data.targetName = target.displayName;
        data.description = target.description;

        photos.Add(data);

        Debug.Log("Saved PNG: " + fullPath);

        return data;
    }

    public void LoadPhotosFromDisk()
    {
        photos.Clear();

        if (!Directory.Exists(folderPath))
            return;

        string[] files = Directory.GetFiles(folderPath, "*.png");

        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            PhotoData data = new PhotoData();
            data.filePath = file;
            data.image = tex;
            data.targetName = "Saved Photo";
            data.description = "Loaded from disk";

            photos.Add(data);
        }

        Debug.Log("Loaded " + photos.Count + " photos");
    }
}