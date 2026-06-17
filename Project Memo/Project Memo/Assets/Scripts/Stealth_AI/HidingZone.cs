using UnityEngine;

namespace VHS
{
    [RequireComponent(typeof(Collider))]
    public class HidingZone : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";

        [Header("When Player Is Inside")]
        [Range(0f, 1f)][SerializeField] private float visibilityMultiplier = 0.35f;
        [Range(0f, 1f)][SerializeField] private float noiseMultiplier = 0.5f;

        private void Reset()
        {
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag))
                return;

            StealthPlayerStatus status = other.GetComponentInParent<StealthPlayerStatus>();

            if (status != null)
                status.SetHidden(true, visibilityMultiplier, noiseMultiplier);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag))
                return;

            StealthPlayerStatus status = other.GetComponentInParent<StealthPlayerStatus>();

            if (status != null)
                status.SetHidden(false, 1f, 1f);
        }
    }
}