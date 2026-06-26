using UnityEngine;

namespace VHS
{
    public class HearingSensor : MonoBehaviour
    {
        [Header("Hearing")]
        [SerializeField] private float hearingMultiplier = 1f;

        [Header("Debug")]
        [SerializeField] private bool drawDebug = true;

        public bool HasHeardNoise { get; private set; }
        public Vector3 LastHeardPosition { get; private set; }
        public float LastHeardRadius { get; private set; }
        public GameObject LastNoiseSource { get; private set; }

        private void OnEnable()
        {
            PlayerNoiseEmitter.NoiseEmitted += OnNoiseEmitted;
        }

        private void OnDisable()
        {
            PlayerNoiseEmitter.NoiseEmitted -= OnNoiseEmitted;
        }

        private void OnNoiseEmitted(Vector3 position, float radius, GameObject source)
        {
            if (source == gameObject)
                return;

            float effectiveRadius = radius * hearingMultiplier;
            float distance = Vector3.Distance(transform.position, position);

            if (distance > effectiveRadius)
                return;

            HasHeardNoise = true;
            LastHeardPosition = position;
            LastHeardRadius = effectiveRadius;
            LastNoiseSource = source;
        }

        public bool ConsumeHeardNoise(out Vector3 heardPosition)
        {
            heardPosition = LastHeardPosition;

            if (!HasHeardNoise)
                return false;

            HasHeardNoise = false;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawDebug || !HasHeardNoise)
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(LastHeardPosition, 0.5f);
            Gizmos.DrawLine(transform.position, LastHeardPosition);
        }
    }
}