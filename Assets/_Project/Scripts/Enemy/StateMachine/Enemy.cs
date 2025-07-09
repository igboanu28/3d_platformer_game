using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Platformer;
using System;

namespace Enemy
{

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {
        [Header("Enemy Type")]
        [SerializeField] private EnemyType type;

        [Header("General Settings")]
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector playerDetector;
        [SerializeField, Child] Animator animator;
        [SerializeField] float wanderRadius = 10f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float idleDuration = 5f;

        [Header("Effects")]
        [SerializeField] private GameObject hitVFXPrefab;
        [SerializeField, Self] EnemyAudio enemyAudio;

        // Ranged enemy
        [Header("Ranged Attack Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        private bool wasHit = false;
        StateMachine stateMachine;
        CountdownTimer attackTimer;
        Platformer.Health health;

        void OnValidate() => this.ValidateRefs();

        void Awake()
        {
            health = GetComponent<Platformer.Health>();
            health.OnDamaged += HandleDamageTaken;
        }

        void OnDestroy()
        {
            health.OnDamaged -= HandleDamageTaken;
        }

        void Start()
        {
            attackTimer = new CountdownTimer(timeBetweenAttacks);

            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            //var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
            var idleState = new EnemyIdleState(this, animator, idleDuration);
            var deathState = new EnemyDeathState(this, animator, agent, enemyAudio);
            var hitState = new EnemyHitState(this, animator, agent, playerDetector.Player, hitVFXPrefab, enemyAudio);

            // this will haelp to set up the correct attack logic based on the dropdown choice.
            if (type == EnemyType.Melee)
            { 
                var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
                At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
                At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            }
            else if (type == EnemyType.Ranged)
            {
                var rangedAttackState = new RangedAttackState(this, animator, agent, playerDetector.Player, 1.5f); // the 1.5s is for the animation length

                // Since the ranged enemy doesn't chase, it attacks from an idle or wander state
                At(wanderState, rangedAttackState, new FuncPredicate(() => playerDetector.CanAttackPlayer() && !attackTimer.IsRunning));
                At(idleState, rangedAttackState, new FuncPredicate(() => playerDetector.CanAttackPlayer() && !attackTimer.IsRunning));

                // When the attack is done go back to wandering
                At(rangedAttackState, wanderState, new FuncPredicate(() => rangedAttackState.IsAttackComplete));
            }

            At(wanderState, chaseState, new FuncPredicate(() => type == EnemyType.Melee && playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            //At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            //At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            At(wanderState, idleState, new FuncPredicate(() => wanderState.IsWanderComplete()));
            At(idleState, wanderState, new FuncPredicate(() => idleState.IsIdleComplete()));

            At(hitState, chaseState, new FuncPredicate(() => hitState.IsHitAnimationComplete && playerDetector.CanDetectPlayer()));
            At(hitState, wanderState, new FuncPredicate(() => hitState.IsHitAnimationComplete && !playerDetector.CanDetectPlayer()));

            // this goes to the death transition from any state
            Any(deathState, new FuncPredicate(() => health.IsDead));
            Any(hitState, new FuncPredicate(() => wasHit && !health.IsDead));

            stateMachine.SetState(wanderState);
        }
        
        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        private void HandleDamageTaken()
        {
            wasHit = true;
        }

        void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);

            if (wasHit)
            {
                wasHit = false;
            }
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        
        public void Attack()
        {
            if (attackTimer.IsRunning) return;
            attackTimer.Start();

            if (type == EnemyType.Melee)
            {
                playerDetector.PlayerHealth.TakeDamage(10);
            }
            
        }

        public void FireProjectile()
        {
            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            }
        }

        private void DestroySelf()
        {
            GameObject.Destroy(gameObject);
        }
    }

    public enum EnemyType
    { 
        Melee,
        Ranged
    }
}