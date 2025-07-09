using UnityEngine;
using Platformer;

namespace Enemy
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 20f;
        [SerializeField] private int damage = 15;
        [SerializeField] private float lifetime = 5f;

        [Header("Effects")]
        [SerializeField] private GameObject hitVFX;
        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<Health>(out var playerHealth))
                {
                    playerHealth.TakeDamage(damage);
                }

                // If we have a hit effect, spawn it at the collision point
                if (hitVFX != null)
                {
                    Instantiate(hitVFX, transform.position, Quaternion.identity);
                }

                // once hit destory 
            }
            Debug.Log($"hit: {other.name}");
            Destroy(gameObject);

        }
    }
}
