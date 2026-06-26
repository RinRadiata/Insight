using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cameraModel;
    public bool holdingCamera = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            holdingCamera = !holdingCamera;
            cameraModel.SetActive(holdingCamera);
        }
    }
}