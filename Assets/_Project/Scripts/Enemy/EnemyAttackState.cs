﻿using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAttackState : EnemyBaseState 
    {
        readonly NavMeshAgent agent;
        readonly Transform player;

        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Attack");
            animator.CrossFade(Cactus_Attack01, crossFadeDuration);
        }

        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }
    }
}
