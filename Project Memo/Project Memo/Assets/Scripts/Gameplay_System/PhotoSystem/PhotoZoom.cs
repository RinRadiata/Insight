using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoZoom : MonoBehaviour
{
    public static PhotoZoom Instance;

    public CanvasGroup canvasGroup;

    public Image zoomImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    bool isOpen = false;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    void Update()
    {
        // close photozoom ui if notebook ui is closed
        if (isOpen && NotebookManager.Instance != null && !NotebookManager.Instance.IsOpen)
        {
            CloseZoom();
        }

        // add esc button to close photozoom
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseZoom();
        }
    }

    public void ShowZoom(PhotoData data)
    {
        if (data == null) return;

        Sprite sprite = Sprite.Create(
            data.image,
            new Rect(0, 0, data.image.width, data.image.height),
            new Vector2(0.5f, 0.5f)
        );

        zoomImage.sprite = sprite;

        titleText.text = data.targetName;
        descriptionText.text = data.description;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isOpen = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isOpen = false;
    }

    public void CloseZoom()
    {
        Hide();

        // if notebook is still open, keep the mouse cursor visible
        if (NotebookManager.Instance != null && NotebookManager.Instance.IsOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}