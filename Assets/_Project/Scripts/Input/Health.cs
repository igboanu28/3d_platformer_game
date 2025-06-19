using UnityEngine;

namespace Platformer
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel enemyHealthChannel;

        int currentHealth;

        public bool IsDead => currentHealth <= 0;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        void Start()
        {
            PublishHealthPercentage();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            PublishHealthPercentage();
        }

        void PublishHealthPercentage()
        {
            if (enemyHealthChannel != null)
                enemyHealthChannel.Invoke(currentHealth / (float)maxHealth);
        }
    }
}