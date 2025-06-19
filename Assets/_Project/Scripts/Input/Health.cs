using System;
using UnityEngine;

namespace Platformer
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel enemyHealthChannel;

        int currentHealth;

        public bool IsDead => currentHealth <= 0;

        public event Action OnDamaged;

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
            if (IsDead) return; // Don't take damage if already dead

            currentHealth -= damage;
            PublishHealthPercentage();

            OnDamaged?.Invoke();
        }

        void PublishHealthPercentage()
        {
            if (enemyHealthChannel != null)
                enemyHealthChannel.Invoke(currentHealth / (float)maxHealth);
        }
    }
}