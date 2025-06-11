using UnityEngine;

namespace Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, UnityEngine.Animator animator) : base(player, animator) { }
        public override void OnEnter() => player.PerformJump();
        public override void FixedUpdate()
        {
            player.HandleMovementAndRotation(); // For air control
            player.HandleGravity();
        }
        public override void OnExit()
        {
            player.JumpHoldTimer.Stop();
            player.CoyoteTimer.Stop();
        }
    }
}
