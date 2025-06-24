using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy
{
    public class EnemyDeathState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly EnemyAudio enemyAudio;

        public EnemyDeathState(Enemy enemy, Animator animator, NavMeshAgent agent, EnemyAudio enemyAudio)
            : base(enemy, animator)
        {
            this.agent = agent;
            this.enemyAudio = enemyAudio;
        }

        public override void OnEnter()
        {
            enemyAudio.PlayDeathSound(); // Play death sound effect

            Debug.Log("Enemy died");

            animator.CrossFade(Cactus_Die, crossFadeDuration);
            agent.isStopped = true;

            // Disable collision and other components if needed
            if (enemy.TryGetComponent<Collider>(out var col))
                col.enabled = false;

            if (enemy.TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;

            // Optionally destroy the enemy after a delay
            enemy.StartCoroutine(DestroyAfterDelay(3f));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
