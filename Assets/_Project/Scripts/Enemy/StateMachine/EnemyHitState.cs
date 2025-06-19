// Create this new script: EnemyHitState.cs
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyHitState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private float hitAnimationDuration;

        public bool IsHitAnimationComplete { get; private set; }

        public EnemyHitState(Enemy enemy, Animator animator, NavMeshAgent agent, float duration = 0.5f)
            : base(enemy, animator)
        {
            this.agent = agent;
            this.hitAnimationDuration = duration;
        }

        public override void OnEnter()
        {
            IsHitAnimationComplete = false;
            agent.isStopped = true; // Stop the enemy from moving while hurt

            // You'll need a Hit animation trigger in your Animator, e.g., "Hit"
            // And an animation clip, e.g., "Cactus_Hit"
            animator.CrossFade("Cactus_GetHit", 0.1f); // Use your actual hit animation name

            // Start a timer to exit the state
            enemy.StartCoroutine(HitCooldown());
        }

        private System.Collections.IEnumerator HitCooldown()
        {
            yield return new WaitForSeconds(hitAnimationDuration);
            IsHitAnimationComplete = true;
        }
    }
}