using UnityEngine;

namespace Platformer
{
    public class DashState : BaseState
    {
        // Reference to the dash timer from PlayerController
        private CountdownTimer dashTimer;

        public DashState(PlayerController player, Animator animator) : base(player, animator)
        {
            // Get the reference to the dashTimer from the PlayerController
            //this.dashTimer = player.dashTimer;
        }

        public override void OnEnter() => player.PerformDash();
        public override void FixedUpdate()
        {
            // Velocity is set once on enter. Gravity is suspended by not calling HandleGravity().
        }

    }
}