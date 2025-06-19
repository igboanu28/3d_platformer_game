using UnityEngine;
using Platformer;

namespace Enemy
{
    public abstract class EnemyBaseState : IState
    {
        protected readonly Enemy enemy;
        protected readonly Animator animator;

        // Get Animation Hashes
        protected static readonly int Cactus_IdlePlant = Animator.StringToHash("Cactus_IdlePlant");
        protected static readonly int Cactus_RunFWD = Animator.StringToHash("Cactus_RunFWD");
        protected static readonly int Cactus_WalkFWD = Animator.StringToHash("Cactus_WalkFWD");
        protected static readonly int Cactus_Attack01 = Animator.StringToHash("Cactus_Attack01");
        protected static readonly int Cactus_GetHit = Animator.StringToHash("Cactus_GetHit");
        protected static readonly int Cactus_Die = Animator.StringToHash("Cactus_Die");
        protected static readonly int Cactus_SenseSomethingMaint = Animator.StringToHash("Cactus_SenseSomethingMaint");

        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(Enemy enemy, Animator animator)
        {
            this.enemy = enemy;
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
