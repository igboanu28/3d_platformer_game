using UnityEngine;
using UnityUtils.StateMachine;


namespace AdvancedController
{
    public class GroundedState : IStates
    {
        readonly AdvancePlayerController controller;

        public GroundedState(AdvancePlayerController controller)
        {
            this.controller = controller;
        }

        public void OnEnter()
        {
            controller.OnGroundContactRegained();
        }
    }

    public class FallingState : IStates
    {
        readonly AdvancePlayerController controller;

        public FallingState(AdvancePlayerController controller)
        {
            this.controller = controller;
        }

        public void OnEnter()
        {
            controller.OnFallStart();
        }
    }

    public class SlidingState : IStates
    {
        readonly AdvancePlayerController controller;

        public SlidingState(AdvancePlayerController controller)
        {
            this.controller = controller;
        }

        public void OnEnter()
        {
            controller.OnGroundContactLost();
        }
    }
    public class RisingState : IStates
    {
        readonly AdvancePlayerController controller;

        public RisingState(AdvancePlayerController controller)
        {
            this.controller = controller;
        }

        public void OnEnter()
        {
            Debug.Log("rising");
            controller.OnGroundContactLost();
        }
    }

    public class JumpingState : IStates
    {
        readonly AdvancePlayerController controller;

        public JumpingState(AdvancePlayerController controller)
        {
            this.controller = controller;
        }

        public void OnEnter()
        {
            controller.OnGroundContactLost();
            controller.OnJumpStart();
        }
    }
}