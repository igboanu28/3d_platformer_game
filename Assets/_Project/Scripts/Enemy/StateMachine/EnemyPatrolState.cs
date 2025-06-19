//using UnityEngine;
//using Platformer;
//using UnityEngine.AI;

//namespace Enemy
//{
//    public class EnemyPatrolState : EnemyBaseState
//    {
//        readonly NavMeshAgent agent;
//        readonly Transform player;
//        Vector3 patrolCenter;
//        Vector3 targetPosition;
//        readonly float patrolRadius;

//        public EnemyPatrolState(Enemy enemy, NavMeshAgent agent, Animator animator, Vector3 patrolCenter, float patrolRadius)
//            : base(enemy, animator)
//        {
//            this.agent = agent;
//            this.patrolCenter = patrolCenter;
//            this.patrolRadius = patrolRadius;
//        }

//        public override void OnEnter()
//        {
//            Debug.Log("Entering Patrol State");
//            animator.CrossFade(Cactus_WalkFWD, crossFadeDuration);
//            SetNewTargetPosition();
//            agent.isStopped = false; // Ensure the agent is active
//        }

//        public override void OnExit()
//        {
//            agent.isStopped = true; // Stop movement on exit
//        }

//        public override void Update()
//        {
//            if (Vector3.Distance(enemy.transform.position, targetPosition) < 1f)
//            {
//                SetNewTargetPosition();
//            }

//            agent.SetDestination(targetPosition);
//        }

//        private void SetNewTargetPosition()
//        {
//            // Generate a random position within the patrol radius
//            Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
//            targetPosition = patrolCenter + new Vector3(randomPoint.x, 0, randomPoint.y);

//            // Ensure the target position is on the NavMesh
//            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
//            {
//                targetPosition = hit.position;
//            }
//            else
//            {
//                Debug.LogWarning("Failed to find a valid NavMesh position for patrol target.");
//            }
//        }
//    }
//}