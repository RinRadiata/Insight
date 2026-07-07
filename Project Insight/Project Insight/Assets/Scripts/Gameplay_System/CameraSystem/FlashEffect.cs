using UnityEngine;
using System.Collections;

public class FlashEffect : MonoBehaviour
{
    public Light flashLight;

    public void TriggerFlash()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        flashLight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        flashLight.enabled = false;
    }
}