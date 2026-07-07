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

            PhotoTarget target = hit.collider.GetComponentInParent<PhotoTarget>();

            if (target != null && target.enabled && target.gameObject.activeInHierarchy)
            {
                Debug.Log("Valid target: " + target.displayName);
                return target;
            }
        }

        return null;
    }
}