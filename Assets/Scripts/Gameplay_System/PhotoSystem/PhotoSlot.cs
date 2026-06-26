using UnityEngine;
using UnityEngine.UI;

public class PhotoSlot : MonoBehaviour
{
    public Image image;

    PhotoData data;

    public void SetPhoto(PhotoData photo)
    {
        data = photo;

        Sprite sprite = Sprite.Create(
            photo.image,
            new Rect(0, 0, photo.image.width, photo.image.height),
            new Vector2(0.5f, 0.5f)
        );

        image.sprite = sprite;
    }

    public void OnClick()
    {
        PhotoZoom.Instance.ShowZoom(data);
    }
}