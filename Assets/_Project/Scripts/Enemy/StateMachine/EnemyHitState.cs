// Create this new script: EnemyHitState.cs
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyHitState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player; // to calculate knockback direction if needed
        private readonly GameObject hitVFXPrefab;
        private readonly EnemyAudio enemyAudio;

        // Knockback parameters
        private readonly float knockbackForce;
        private readonly float knockbackDuration;
        private Vector3 knockbackDirection;
        private float knockbackTimer;
        
        private float hitAnimationDuration;
        public bool IsHitAnimationComplete { get; private set; }

        public EnemyHitState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player, GameObject hitVFXPrefab, EnemyAudio enemyAudio, float duration = 0.5f, float knockbackForce = 5f, float knockbackDuration = 0.2f)
            : base(enemy, animator)
        {
            this.enemyAudio = enemyAudio;
            this.agent = agent;
            this.player = player;
            this.hitVFXPrefab = hitVFXPrefab;
            this.hitAnimationDuration = duration;
            this.knockbackForce = knockbackForce;
            this.knockbackDuration = knockbackDuration;
        }

        public override void OnEnter()
        {
            enemyAudio.PlayHitSound(); // Play hit sound effect

            IsHitAnimationComplete = false;
            agent.isStopped = true; // Stop the enemy from moving while hurt

            animator.CrossFade("Cactus_GetHit", 0.22f);

            // Calculate knockback direction
            if (player != null)
            {
                // Direction from player to us 
                knockbackDirection = (enemy.transform.position - player.position).normalized;
                knockbackDirection.y = 0; // don't knock the enemy up/down
                knockbackTimer = knockbackDuration; 
            }

            if (hitVFXPrefab != null)
            {
                Object.Instantiate(hitVFXPrefab, enemy.transform.position, Quaternion.identity);
            }

            // Start a timer to exit the state
            enemy.StartCoroutine(HitCooldown());
        }

        public override void Update()
        {
            if (knockbackTimer > 0)
            {
                // Move the agent manually. This respects the NavMesh.
                agent.Move(knockbackDirection * knockbackForce * Time.deltaTime);
                knockbackTimer -= Time.deltaTime;
            }
        }

        public override void OnExit()
        {
            // Re-enable the agent's normal pathfinding behavior when exiting the state
            agent.isStopped = false;
        }

        private System.Collections.IEnumerator HitCooldown()
        {
            yield return new WaitForSeconds(hitAnimationDuration);
            IsHitAnimationComplete = true;
        }
    }
}