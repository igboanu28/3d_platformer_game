using Platformer;
using UnityEngine;

namespace CombatSystem
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    public class EnemyResponder : MonoBehaviour
    {
        private Health _health;
        private Animator _animator; // optional, for animations

        void Awake()
        {
            _health = GetComponent<Health>();
            _animator = GetComponent<Animator>();
        }

        void OnEnable()
        {
            if (_health != null)
            { 
                _health.OnDamageTakenWithContext += HandleDamgeTaken;
                _health.OnDeath += HandleDeath;
            }
        }

        void OnDisable()
        {
            if (_health != null)
            { 
                _health.OnDamageTakenWithContext -= HandleDamgeTaken;
                _health.OnDeath -= HandleDeath;
            }
        }

        private void HandleDamgeTaken(float damageAmount, AttackSO sourceAttackData)
        {
            Debug.Log($"{gameObject.name} took {damageAmount} damage from {sourceAttackData?.name ?? "unknown source"}.");

            if (_animator != null)
            {
                _animator.SetTrigger("Hit"); // Trigger a hit animation if available
            }

            if (FeedbackManager.Instance != null) // Play a generic enemy hit sound
            {
                //FeedbackManager.Instance.PlaySFX(enemyHitSound, transform.position);
            }
        }

        private void HandleDeath()
        {
            if (_animator != null)
            { 
                _animator.SetTrigger("Die"); // Trigger a death animation if available
            }

            // Disable colliders, AI, etc.
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false; // Disable all colliders
            }
        }
    }
}