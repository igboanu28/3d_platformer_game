using Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyDeathState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        public EnemyDeathState(Enemy enemy, Animator animator, NavMeshAgent agent) : base(enemy, animator)
        {
            this.agent = agent;
        }

        public override void OnEnter()
        {
            Debug.Log("Enemy is Dead");
            animator.CrossFade(Cactus_Die, crossFadeDuration);
            agent.isStopped = true;
            enemy.gameObject.SetActive(false); // Or Destroy(enemy.gameObject) for boss enemies
        }
    }
}
