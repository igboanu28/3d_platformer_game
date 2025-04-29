using Platformer;
using UnityEngine;

namespace CompanionSystem
{
    public abstract class CompanionBaseState : IState
    {
        protected readonly Companion companion;
        protected readonly Animator animator;

        // Get Animation Hashes
        protected static readonly int Mushroom_IdleNormalSmile = Animator.StringToHash("Mushroom_IdleNormalSmile");
        protected static readonly int Mushroom_runFWDSmile = Animator.StringToHash("Mushroom_runFWDSmile");
        protected static readonly int Mushroom_Attack02Smile = Animator.StringToHash("Mushroom_Attack02Smile");

        // Animation crossfade duration
        protected const float crossFadeDuration = 0.1f;

        protected CompanionBaseState(Companion companion, Animator animator)
        {
            this.companion = companion;
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }
    }
}
