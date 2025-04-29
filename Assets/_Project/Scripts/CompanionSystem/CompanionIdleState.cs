using UnityEngine;

namespace CompanionSystem
{
    public class CompanionIdleState : CompanionBaseState 
    {
        private readonly Transform player;
        private readonly float followHeight;

        public CompanionIdleState(Companion companion, Animator animator, Transform player, float followHeight) : base(companion, animator)
        {
            this.player = player;
            this.followHeight = followHeight;
        }

        public override void OnEnter()
        {
            Debug.Log("Companion in Idle state");
            animator.CrossFade(Mushroom_IdleNormalSmile, crossFadeDuration);
        }

        public override void Update()
        { 
            // Hover or Idle over player head
            Vector3 hoverPosition = player.position;
            hoverPosition.y += followHeight;
            companion.transform.position = Vector3.Lerp(companion.transform.position, hoverPosition, Time.deltaTime * 2f);
        }
    }
}
