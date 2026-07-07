using System.Collections.Generic;
using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public static NotebookManager Instance;

    public List<PhotoData> entries = new List<PhotoData>();

    public CanvasGroup notebookUI;

    public bool IsOpen { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CloseNotebook();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleNotebook();
        }
    }

    public void ToggleNotebook()
    {
        if (IsOpen)
            CloseNotebook();
        else
            OpenNotebook();
    }

    public void OpenNotebook()
    {
        notebookUI.alpha = 1;
        notebookUI.interactable = true;
        notebookUI.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        IsOpen = true;
    }

    public void CloseNotebook()
    {
        notebookUI.alpha = 0;
        notebookUI.interactable = false;
        notebookUI.blocksRaycasts = false;

        // close photozoom if its still open
        if (PhotoZoom.Instance != null)
            PhotoZoom.Instance.CloseZoom();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        IsOpen = false;
    }

    public void AddEntry(PhotoData data)
    {
        entries.Add(data);
        Debug.Log("Notebook entry added: " + data.targetName);
    }
}