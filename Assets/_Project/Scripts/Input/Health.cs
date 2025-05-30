﻿using JetBrains.Annotations;
using UnityEngine;

namespace Platformer
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel playerHealthChannel;

        int currentHealth;

        public bool IsDead => currentHealth <= 0;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        void Start()
        {
            //PublishHealthPercentage();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            //PublishHealthPercentage();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            
        }

        //void PublishHealthPercentage()
        //{
        //    if (playerHealthChannel != null)
        //        playerHealthChannel.Invoke(currentHealth / (float)maxHealth);
        //}
    }
}
