using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class FireballAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;
        private readonly float animationDuration;

        public bool IsAttackComplete { get; private set; }

        public FireballAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player, float animationDuration) : base(enemy, animator)
        { 
            this.agent = agent;
            this.player = player;
            this.animationDuration = animationDuration;
        }

        public override void OnEnter()
        {
            IsAttackComplete = false;
            agent.isStopped = true;

            // this will make the enemy look at the player
            Vector3 lookDirection = player.position - enemy.transform.position;
            lookDirection.y = 0;
            enemy.transform.rotation = Quaternion.LookRotation(lookDirection);

            animator.CrossFade(Fireball_Attack, crossFadeDuration);

            

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
