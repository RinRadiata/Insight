using UnityEngine;

namespace VHS
{
    [RequireComponent(typeof(Rigidbody))]
    public class NoiseOnCollision : MonoBehaviour
    {
        [SerializeField] private float minImpactForce = 2f;
        [SerializeField] private float maxImpactForce = 10f;
        [SerializeField] private float maxNoiseRadius = 12f;
        [SerializeField] private float cooldown = 0.4f;

        private float nextNoiseTime;

        private void OnCollisionEnter(Collision collision)
        {
            if (Time.time < nextNoiseTime)
                return;

            float impact = collision.relativeVelocity.magnitude;

            if (impact < minImpactForce)
                return;

            float t = Mathf.InverseLerp(minImpactForce, maxImpactForce, impact);
            float radius = Mathf.Lerp(maxNoiseRadius * 0.35f, maxNoiseRadius, t);

            Vector3 noisePosition = transform.position;

            if (collision.contactCount > 0)
                noisePosition = collision.GetContact(0).point;

            PlayerNoiseEmitter.EmitNoise(noisePosition, radius, gameObject);
            nextNoiseTime = Time.time + cooldown;
        }
    }
}