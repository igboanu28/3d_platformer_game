using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Enemy
{
    public class EnemyHitState : EnemyBaseState
    {
        private float hitDuration;
        private CountdownTimer hitTimer;

        public EnemyHitState(Enemy enemy, Animator animator, float hitDuration)
            : base(enemy, animator)
        {
            this.hitDuration = hitDuration;
            this.hitTimer = new CountdownTimer(hitDuration);
        }

        public override void OnEnter()
        {
            Debug.Log("Enemy got hit");
            animator.CrossFade(Cactus_GetHit, crossFadeDuration);
            hitTimer.Reset();
            hitTimer.Start();
        }

        public override void Update()
        {
            hitTimer.Tick(Time.deltaTime);
        }

        public bool IsHitAnimationComplete()
        {
            return hitTimer.IsFinished;
        }
    }
}
