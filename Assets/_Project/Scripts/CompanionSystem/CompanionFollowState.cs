using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace CompanionSystem
{
    public class CompanionFollowState : CompanionBaseState
    {
        private readonly Transform player;
        private readonly float followHeight;
        private readonly float followDistance;
        private readonly float moveSpeed;
        private readonly float wallAvoidanceRadius;
        private readonly LayerMask ground;

        public CompanionFollowState(Companion companion, Animator animator, Transform player, float followHeight, float followDistance, float moveSpeed, float wallAvoidanceRadius, LayerMask ground) : base(companion, animator)
        {
            this.player = player;
            this.followHeight = followHeight;
            this.followDistance = followDistance;
            this.moveSpeed = moveSpeed;
            this.wallAvoidanceRadius = wallAvoidanceRadius;
            this.ground = ground;
        }

        public override void OnEnter()
        {
            Debug.Log("Companion Entering follow state");
            animator.CrossFade(Mushroom_runFWDSmile, crossFadeDuration);
        }

        public override void Update()
        {
            Vector3 targetPosition = player.position - player.forward * followDistance;
            targetPosition.y += followHeight;

            // Avoid walls
            if (Physics.CheckSphere(targetPosition, wallAvoidanceRadius, ground))
            {
                targetPosition += player.right * wallAvoidanceRadius; // Adjust position to avoid walls
            }

            // Move the companion
            companion.transform.position = Vector3.MoveTowards(companion.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
