using UnityEngine;

namespace CombatSystem
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private int damage = 10;
        [SerializeField] private string enemyTag = "Enemy";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enemyTag))
            {
                if (other.TryGetComponent<Platformer.Health>(out var health))
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}