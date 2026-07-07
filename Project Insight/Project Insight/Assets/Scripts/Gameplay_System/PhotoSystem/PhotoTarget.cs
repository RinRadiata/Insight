using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    public string targetID;
    public string displayName;
    [TextArea]
    public string description;

    public int scoreValue = 10;

    public bool discovered = false;
}