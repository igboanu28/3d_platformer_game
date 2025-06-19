using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyWanderState : EnemyBaseState 
    {
        readonly NavMeshAgent agent;
        readonly Vector3 startPoint;
        readonly float wanderRadius;
        bool wanderComplete;
        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRaduis) : base(enemy, animator)
        {
            this.agent = agent;
            this.startPoint = enemy.transform.position;
            this.wanderRadius = wanderRaduis;
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Wander State");
            animator.CrossFade(Cactus_WalkFWD, crossFadeDuration);
            MoveToRandomPoint();
            wanderComplete = false;
        }

        public override void Update()
        {
            if (HasReachedDestination())
            {
                wanderComplete = true;
            }
        }

        public bool IsWanderComplete()
        {
            return wanderComplete;
        }

        private void MoveToRandomPoint()
        {
            var randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += startPoint;
            NavMeshHit hit;
            // this ensure the point is on the navMesh
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            { 
                agent.SetDestination(hit.position);
            }
            //var finalPosition = hit.position;

            //agent.SetDestination(finalPosition);
        }

        bool HasReachedDestination()
        {
            return !agent.pathPending
                && agent.remainingDistance <= agent.stoppingDistance
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
}
