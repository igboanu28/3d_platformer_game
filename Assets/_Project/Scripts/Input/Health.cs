using CombatSystem;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Platformer
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] private float maxHealth = 100f;
        public float CurrentHealth { get; private set; }

        // Event parameters: actual damage taken, source AttackData (can be null for environmental damage etc.)
        public event Action<float, AttackSO> OnDamageTakenWithContext;
        public event Action OnDeath;
        public bool IsDead { get; private set; }

        void Awake()
        {
            CurrentHealth = maxHealth;
        }

        // Call this to deal damage
        public void TakeDamage(float amount, AttackSO sourceAttackData = null)
        {
            if (IsDead) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Max(CurrentHealth, 0); // Prevent negative health

            OnDamageTakenWithContext?.Invoke(amount, sourceAttackData);
            // Debug.Log($"{gameObject.name} took {amount} damage. Health: {CurrentHealth}");

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        //public void Heal(float amount)
        //{
        //    if (IsDead) return;
        //    CurrentHealth += amount;
        //    CurrentHealth = Mathf.Min(CurrentHealth, maxHealth); // Prevent over-healing
        //                                                         // Debug.Log($"{gameObject.name} healed {amount}. Health: {CurrentHealth}");
        //}

        private void Die()
        {
            if (IsDead) return; // Ensure Die is only called once
            IsDead = true;
            OnDeath?.Invoke();
            // Debug.Log($"{gameObject.name} has died.");
            // Add logic here for what happens on death (e.g., play animation, disable components, game over)
        }
    }
}
