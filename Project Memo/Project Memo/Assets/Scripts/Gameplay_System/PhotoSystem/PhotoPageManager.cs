using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoPageManager : MonoBehaviour
{
    public Transform photoGrid;
    public GameObject photoSlotPrefab;

    public Transform pageBar;
    public GameObject pageButtonPrefab;

    public int photosPerPage = 6;

    int currentPage = 0;

    List<PhotoData> photos
    {
        get
        {
            if (PhotoManager.Instance == null)
                return new List<PhotoData>();

            return PhotoManager.Instance.photos;
        }
    }

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        GeneratePageButtons();
        ShowPage(0);
    }

    void GeneratePageButtons()
    {
        if (pageBar == null) return;

        foreach (Transform child in pageBar)
            Destroy(child.gameObject);

        if (photos.Count == 0)
            return;

        int pageCount = Mathf.CeilToInt((float)photos.Count / photosPerPage);

        for (int i = 0; i < pageCount; i++)
        {
            int pageIndex = i;

            GameObject btn = Instantiate(pageButtonPrefab, pageBar);

            TMP_Text text = btn.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = (i + 1).ToString();

            Button button = btn.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    ShowPage(pageIndex);
                });
            }
        }
    }

    public void ShowPage(int page)
    {
        if (photoGrid == null) return;

        currentPage = page;

        foreach (Transform child in photoGrid)
            Destroy(child.gameObject);

        int start = page * photosPerPage;
        int end = Mathf.Min(start + photosPerPage, photos.Count);

        for (int i = start; i < end; i++)
        {
            GameObject slot = Instantiate(photoSlotPrefab, photoGrid);

            PhotoSlot photoSlot = slot.GetComponent<PhotoSlot>();

            if (photoSlot != null)
                photoSlot.SetPhoto(photos[i]);
        }
    }
}