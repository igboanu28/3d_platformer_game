using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyDeathState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;

        public EnemyDeathState(Enemy enemy, Animator animator, NavMeshAgent agent)
            : base(enemy, animator)
        {
            this.agent = agent;
        }

        public override void OnEnter()
        {
            Debug.Log("Enemy died");

            animator.CrossFade(Cactus_Die, crossFadeDuration);
            agent.isStopped = true;

            // Disable collision and other components if needed
            if (enemy.TryGetComponent<Collider>(out var col))
                col.enabled = false;

            if (enemy.TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;

            // Optionally destroy the enemy after a delay
            enemy.Invoke(nameof(DestroySelf), 3f);
        }

        private void DestroySelf()
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
