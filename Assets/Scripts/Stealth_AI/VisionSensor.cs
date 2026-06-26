using UnityEngine;

namespace VHS
{
    public class VisionSensor : MonoBehaviour
    {
        [Header("Vision")]
        [SerializeField] private Transform eye;
        [SerializeField] private float viewRadius = 12f;
        [Range(1f, 180f)][SerializeField] private float viewAngle = 80f;
        [SerializeField] private string targetTag = "Player";

        [Header("Masks")]
        [SerializeField] private LayerMask targetMask = ~0;
        [SerializeField] private LayerMask obstacleMask = ~0;

        [Header("Debug")]
        [SerializeField] private bool drawDebug = true;

        public Transform VisibleTarget { get; private set; }

        public bool CanSeeTarget(Transform target)
        {
            VisibleTarget = null;

            if (target == null)
                return false;

            Vector3 origin = GetEyePosition();
            Vector3 targetPoint = target.position + Vector3.up;
            Vector3 directionToTarget = targetPoint - origin;
            float distanceToTarget = directionToTarget.magnitude;

            StealthPlayerStatus stealthStatus = target.GetComponentInParent<StealthPlayerStatus>();
            float visibilityMultiplier = stealthStatus != null ? stealthStatus.VisibilityMultiplier : 1f;

            if (visibilityMultiplier <= 0.01f)
                return false;

            float effectiveRadius = viewRadius * visibilityMultiplier;

            if (distanceToTarget > effectiveRadius)
                return false;

            float angle = Vector3.Angle(GetEyeForward(), directionToTarget.normalized);

            if (angle > viewAngle * 0.5f)
                return false;

            if (!HasLineOfSight(origin, directionToTarget.normalized, distanceToTarget, target))
                return false;

            VisibleTarget = target;
            return true;
        }

        public bool TryFindVisibleTarget(out Transform target)
        {
            target = null;
            VisibleTarget = null;

            Collider[] hits = Physics.OverlapSphere(GetEyePosition(), viewRadius, targetMask, QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                if (!hit.CompareTag(targetTag))
                    continue;

                Transform candidate = hit.transform;

                if (CanSeeTarget(candidate))
                {
                    target = candidate;
                    return true;
                }
            }

            return false;
        }

        private bool HasLineOfSight(Vector3 origin, Vector3 direction, float distance, Transform target)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, distance, obstacleMask, QueryTriggerInteraction.Ignore);

            if (hits == null || hits.Length == 0)
                return true;

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == null)
                    continue;

                if (hit.transform.root == transform.root)
                    continue;

                if (hit.transform == target || hit.transform.IsChildOf(target) || target.IsChildOf(hit.transform))
                    return true;

                return false;
            }

            return true;
        }

        private Vector3 GetEyePosition()
        {
            if (eye != null)
                return eye.position;

            return transform.position + Vector3.up * 1.6f;
        }

        private Vector3 GetEyeForward()
        {
            if (eye != null)
                return eye.forward;

            return transform.forward;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawDebug)
                return;

            Vector3 origin = GetEyePosition();
            Vector3 forward = GetEyeForward();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, viewRadius);

            Vector3 left = Quaternion.Euler(0f, -viewAngle * 0.5f, 0f) * forward;
            Vector3 right = Quaternion.Euler(0f, viewAngle * 0.5f, 0f) * forward;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, left * viewRadius);
            Gizmos.DrawRay(origin, right * viewRadius);

            if (VisibleTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, VisibleTarget.position + Vector3.up);
            }
        }
    }
}