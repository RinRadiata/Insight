using UnityEngine;

public class PhotoDetector : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 50f;

    public PhotoTarget DetectTarget()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            PhotoTarget target = hit.collider.GetComponent<PhotoTarget>();

            if (target != null)
            {
                return target;
            }
        }

        return null;
    }
}