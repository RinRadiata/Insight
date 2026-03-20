using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance;

    public List<PhotoData> photos = new List<PhotoData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PhotoData AddPhoto(Texture2D image, PhotoTarget target)
    {
        PhotoData data = new PhotoData();

        data.image = image;
        data.targetName = target.displayName;
        data.description = target.description;

        photos.Add(data);

        Debug.Log("Memo saved: " + target.displayName);

        return data;
    }
}