using UnityEngine;
using UnityEngine.AI;
using KBCore.Refs;
using Platformer;

namespace Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        readonly float idleDuration;
        private CountdownTimer idleTimer;

        public EnemyIdleState(Enemy enemy, Animator animator, float idleDuartion): base(enemy, animator)
        {
            this.idleDuration = idleDuartion;
            idleTimer = new CountdownTimer(idleDuration);
        }

        public override void OnEnter()
        {
            Debug.Log("Idle - Looking Around");
            animator.CrossFade(Cactus_SenseSomethingMaint, crossFadeDuration);
            idleTimer.Reset();
            idleTimer.Start();
        }

        public override void Update()
        {
            idleTimer.Tick(Time.deltaTime);
        }

        public bool IsIdleComplete()
        {
            return idleTimer.IsFinished;
        }
    }
}