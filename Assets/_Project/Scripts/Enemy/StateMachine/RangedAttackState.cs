using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy
{
    public class RangedAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;
        private readonly float animationDuration;
        public bool IsAttackComplete { get; private set; }

        public RangedAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player, float animationDuration)
            : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
            this.animationDuration = animationDuration;
        }

        public override void OnEnter()
        {
            IsAttackComplete = false;
            agent.isStopped = true;

            // Turn to face the player before attacking
            Vector3 lookDirection = player.position - enemy.transform.position;
            lookDirection.y = 0;
            enemy.transform.rotation = Quaternion.LookRotation(lookDirection);

            // Play the Ranged Attack animation
            animator.CrossFade(Fireball_Attack, crossFadeDuration);

            // Start the main attack cooldown
            enemy.Attack();

            // Start a local timer to know when the animation is finished
            enemy.StartCoroutine(AttackTimer());
        }

        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(animationDuration);
            IsAttackComplete = true;
        }

        public override void OnExit()
        {
            agent.isStopped = false;
        }
    }
}