using UnityEngine;

namespace VHS
{
    public class StealthPlayerStatus : MonoBehaviour
    {
        [Header("Runtime Status")]
        [SerializeField] private bool isHidden;

        [Header("Stealth Multipliers")]
        [Range(0f, 1f)][SerializeField] private float visibilityMultiplier = 1f;
        [Range(0f, 1f)][SerializeField] private float noiseMultiplier = 1f;

        public bool IsHidden
        {
            get => isHidden;
            set => isHidden = value;
        }

        public float VisibilityMultiplier
        {
            get => visibilityMultiplier;
            set => visibilityMultiplier = Mathf.Clamp01(value);
        }

        public float NoiseMultiplier
        {
            get => noiseMultiplier;
            set => noiseMultiplier = Mathf.Clamp01(value);
        }

        public void SetHidden(bool hidden, float newVisibilityMultiplier, float newNoiseMultiplier)
        {
            isHidden = hidden;
            visibilityMultiplier = hidden ? Mathf.Clamp01(newVisibilityMultiplier) : 1f;
            noiseMultiplier = hidden ? Mathf.Clamp01(newNoiseMultiplier) : 1f;
        }
    }
}