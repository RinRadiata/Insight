using UnityEngine;
using System;
using System.Collections;

public class PhotoScreenshot : MonoBehaviour
{
    public IEnumerator CaptureScreenshot(Action<Texture2D> callback)
    {
        // wait for the end of the frame to ensure the screen is fully rendered
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        callback?.Invoke(screenshot);
    }
}