using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup menuUI;

    public CanvasGroup mainPanel;
    public CanvasGroup questPanel;
    public CanvasGroup saveLoadPanel;

    public QuestListUI questListUI;
    public SaveLoadUI saveLoadUI;

    bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }
    public bool IsOpen()
    {
        return isOpen;
    }

    public void Toggle()
    {
        isOpen = !isOpen;

        SetPanel(menuUI, isOpen);

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        Time.timeScale = isOpen ? 0 : 1;

        ShowMain();
    }

    void ShowMain()
    {
        SetPanel(mainPanel, true);
        SetPanel(questPanel, false);
        SetPanel(saveLoadPanel, false);
    }

    public void Resume()
    {
        Toggle();
    }

    public void OpenQuest()
    {
        SetPanel(mainPanel, false);
        SetPanel(questPanel, true);
        SetPanel(saveLoadPanel, false);

        questListUI.Show(QuestManager.Instance.quests);
    }

    public void OpenSaveLoad()
    {
        SetPanel(mainPanel, false);
        SetPanel(questPanel, false);
        SetPanel(saveLoadPanel, true);

        if (saveLoadUI != null)
        {
            saveLoadUI.Refresh();
        }
    }

    void SetPanel(CanvasGroup cg, bool show)
    {
        if (cg == null) return;

        cg.alpha = show ? 1 : 0;
        cg.interactable = show;
        cg.blocksRaycasts = show;
    }

    public void Back()
    {
        ShowMain();
    }

    public void Quit()
    {
        Application.Quit();
    }
}