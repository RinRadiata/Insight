using System;
using UnityEngine;

namespace VHS
{
    public class PlayerNoiseEmitter : MonoBehaviour
    {
        public static event Action<Vector3, float, GameObject> NoiseEmitted;

        [Header("Input")]
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        [Header("Noise Radius")]
        [SerializeField] private float crouchNoiseRadius = 1f;
        [SerializeField] private float walkNoiseRadius = 3f;
        [SerializeField] private float runNoiseRadius = 8f;
        [SerializeField] private float jumpNoiseRadius = 10f;

        [Header("Timing")]
        [SerializeField] private float movementNoiseInterval = 0.35f;

        [Header("Optional")]
        [SerializeField] private StealthPlayerStatus stealthStatus;

        private float noiseTimer;

        private void Awake()
        {
            if (stealthStatus == null)
                stealthStatus = GetComponent<StealthPlayerStatus>();
        }

        private void Update()
        {
            HandleMovementNoise();
            HandleJumpNoise();
        }

        private void HandleMovementNoise()
        {
            float h = Input.GetAxisRaw(horizontalAxis);
            float v = Input.GetAxisRaw(verticalAxis);
            Vector2 input = new Vector2(h, v);

            if (input.sqrMagnitude <= 0.01f)
            {
                noiseTimer = movementNoiseInterval;
                return;
            }

            noiseTimer += Time.deltaTime;

            if (noiseTimer < movementNoiseInterval)
                return;

            noiseTimer = 0f;

            float radius = walkNoiseRadius;

            if (Input.GetKey(runKey))
                radius = runNoiseRadius;

            if (Input.GetKey(crouchKey))
                radius = crouchNoiseRadius;

            EmitNoise(radius);
        }

        private void HandleJumpNoise()
        {
            if (Input.GetKeyDown(jumpKey))
                EmitNoise(jumpNoiseRadius);
        }

        public void EmitNoise(float radius)
        {
            float multiplier = stealthStatus != null ? stealthStatus.NoiseMultiplier : 1f;
            EmitNoise(transform.position, radius * multiplier, gameObject);
        }

        public static void EmitNoise(Vector3 position, float radius, GameObject source)
        {
            if (radius <= 0f)
                return;

            NoiseEmitted?.Invoke(position, radius, source);
        }
    }
}