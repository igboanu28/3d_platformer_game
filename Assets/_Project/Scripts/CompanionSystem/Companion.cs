using Platformer;
using System;
using UnityEngine;

namespace CompanionSystem
{
    public class Companion : Entity 
    {
        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;
        [SerializeField] private float followHeight = 2f;
        [SerializeField] private float followDistance = 1.5f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float wallAvoidanceRadius = 1f;
        [SerializeField] private LayerMask ground;

        StateMachine stateMachine;

        void Start()
        {
            stateMachine = new StateMachine();

            var followState = new CompanionFollowState(this, animator,  player,  followHeight, followDistance, moveSpeed, wallAvoidanceRadius, ground);
            var idleState = new CompanionIdleState(this, animator, player, followHeight);

            //At(idleState, followState, new FuncPredicate(() => player.GetComponent<PlayerController>().IsMoving));
            //At(followState, idleState, new FuncPredicate(() => !player.GetComponent<PlayerController>().IsMoving));

            stateMachine.SetState(idleState);
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);

        void Update()
        {
            stateMachine.Update();
        }
    }
}
