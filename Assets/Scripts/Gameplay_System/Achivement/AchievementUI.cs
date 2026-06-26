using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    public static AchievementUI Instance;

    public CanvasGroup canvasGroup;

    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public float fadeDuration = 0.3f;
    public float displayTime = 2f;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;
    }

    public void Show(string title, string description)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(title, description));
    }

    IEnumerator ShowRoutine(string title, string desc)
    {
        titleText.text = title;
        descriptionText.text = desc;

        // fade in
        yield return StartCoroutine(Fade(0, 1));

        // wait
        yield return new WaitForSeconds(displayTime);

        // fade out
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}