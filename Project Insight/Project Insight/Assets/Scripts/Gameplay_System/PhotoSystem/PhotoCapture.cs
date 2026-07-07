using UnityEngine;
using System.Collections;

public class PhotoCapture : MonoBehaviour
{
    public PhotoDetector detector;
    public PhotoScreenshot screenshot;

    void Update()
    {
        if (PauseMenuExistsAndOpen())
            return;

        //we cant do stuff if the notebook ui is open
        if (NotebookManager.Instance != null && NotebookManager.Instance.IsOpen)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            TakePhoto();
        }
    }

    bool PauseMenuExistsAndOpen()
    {
        PauseMenu pm = FindFirstObjectByType<PauseMenu>();
        return pm != null && pm.IsOpen();
    }

    void TakePhoto()
    {
        if (detector == null || screenshot == null)
        {
            Debug.LogWarning("PhotoCapture: Missing references.");
            return;
        }

        PhotoTarget target = detector.DetectTarget();

        if (target == null)
        {
            Debug.Log("Nothing interesting in this Memo.");
            return;
        }

        StartCoroutine(screenshot.CaptureScreenshot((image) =>
        {
            if (image == null)
            {
                Debug.LogWarning("Screenshot failed.");
                return;
            }

            // save photo
            PhotoData data = PhotoManager.Instance.AddPhoto(image, target);

            // notebook entry
            if (NotebookManager.Instance != null)
                NotebookManager.Instance.AddEntry(data);

            //fk sitting here for 2 hours to realize just to add this lines
            if (AchievementManager.Instance != null)
                AchievementManager.Instance.CheckAchievements();

            // refresh gallery
            PhotoPageManager gallery = FindFirstObjectByType<PhotoPageManager>();

            if (gallery != null)
                gallery.Refresh();

            Debug.Log("Memo captured: " + target.displayName);
        }));

        QuestManager.Instance.AddProgress(1);
    }
}