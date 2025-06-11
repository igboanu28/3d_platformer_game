using DialogueSystem;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using CombatSystem;
using System.Runtime.CompilerServices;

namespace Platformer
{

    /* so if you are confused about the validatedMonoBehaviour it's the open source library that i added from github
     So it basically allows me to validate and serialize all the references in the validate method 
       and that way we don't have to do a bunch of get components calls in awake or start which can have a fairly affect
        on my scene load time 
     */
    public class PlayerController : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private Animator animator;
        [SerializeField] private InputReader inputReader; // YOUR InputReader script
        [SerializeField] private PlayerCombat playerCombat;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 1080f;
        [SerializeField] private float movementSmoothTime = 0.1f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float jumpHoldDuration = 0.35f;
        [SerializeField] private float jumpCooldownDuration = 0.2f;
        [SerializeField] private float coyoteTimeDuration = 0.15f;
        [SerializeField] private float jumpBufferDuration = 0.15f;
        [SerializeField] private float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] private float dashForce = 20f;
        [SerializeField] private float dashDuration = 0.25f;
        [SerializeField] private float dashCooldownDuration = 0.8f;

        // Timers
        public CountdownTimer JumpHoldTimer { get; private set; }
        public CountdownTimer JumpCooldownTimer { get; private set; }
        public CountdownTimer CoyoteTimer { get; private set; }
        public CountdownTimer JumpBufferTimer { get; private set; }
        public CountdownTimer DashTimer { get; private set; }
        public CountdownTimer DashCooldownTimer { get; private set; }
        private List<Timer> _timers;

        // Public Properties for States
        public Rigidbody Rb => rb;
        public Animator Animator => animator;
        public GroundChecker GroundChecker => groundChecker;
        public PlayerCombat PlayerCombat => playerCombat;
        public Vector3 MovementInput { get; private set; }

        private Transform _mainCamTransform;
        private StateMachine _stateMachine;
        private Vector3 _targetMoveDirection;
        private float _currentHorizontalSpeed;
        private float _speedSmoothVelocity;
        private bool _isDashBuffered = false;

        // Animator Hashes
        private static readonly int AnimSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int AnimJumpTrigger = Animator.StringToHash("Jump");
        private static readonly int AnimDashTrigger = Animator.StringToHash("Dash");

        void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (groundChecker == null) groundChecker = GetComponent<GroundChecker>();
            if (animator == null) animator = GetComponent<Animator>();
            if (playerCombat == null) playerCombat = GetComponent<PlayerCombat>();
            if (inputReader == null) Debug.LogError("InputReader NOT ASSIGNED in PlayerController!", this);

            _mainCamTransform = Camera.main?.transform;
            rb.freezeRotation = true;

            SetupTimers();
            SetupStateMachine();
        }

        private void OnEnable()
        {
            if (inputReader == null) return;
            inputReader.Move += OnMove;
            inputReader.Jump += OnJumpInput;
            inputReader.Dash += OnDashInput;
            inputReader.LightAttack += OnLightAttack;
            inputReader.HeavyAttack += OnHeavyAttack;
            //if (groundChecker != null) groundChecker.OnLeftGround += StartCoyoteTimer;
        }

        private void OnDisable()
        {
            if (inputReader == null) return;
            inputReader.Move -= OnMove;
            inputReader.Jump -= OnJumpInput;
            inputReader.Dash -= OnDashInput;
            inputReader.LightAttack -= OnLightAttack;
            inputReader.HeavyAttack -= OnHeavyAttack;
            //if (groundChecker != null) groundChecker.OnLeftGround -= StartCoyoteTimer;
        }

        void Update()
        {
            foreach (var timer in _timers) timer.Tick(Time.deltaTime);
            _stateMachine.Update(); // Use your StateMachine's Update method
            UpdateAnimator();
        }

        void FixedUpdate()
        {
            _stateMachine.FixedUpdate(); // Use your StateMachine's FixedUpdate method
        }

        void SetupTimers()
        {
            JumpHoldTimer = new CountdownTimer(jumpHoldDuration);
            JumpCooldownTimer = new CountdownTimer(jumpCooldownDuration);
            CoyoteTimer = new CountdownTimer(coyoteTimeDuration);
            JumpBufferTimer = new CountdownTimer(jumpBufferDuration);
            DashTimer = new CountdownTimer(dashDuration);
            DashCooldownTimer = new CountdownTimer(dashCooldownDuration);
            _timers = new List<Timer> { JumpHoldTimer, JumpCooldownTimer, CoyoteTimer, JumpBufferTimer, DashTimer, DashCooldownTimer };
            JumpHoldTimer.OnTimerStop += JumpCooldownTimer.Start;
            DashTimer.OnTimerStop += DashCooldownTimer.Start;
        }

        void SetupStateMachine()
        {
            _stateMachine = new StateMachine();
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            var attackState = new AttackState(this, playerCombat);

            At(locomotionState, jumpState, new FuncPredicate(CanJump));
            At(locomotionState, dashState, new FuncPredicate(() => _isDashBuffered && CanDash()));
            At(locomotionState, attackState, new FuncPredicate(() => playerCombat.IsAttacking));

            At(jumpState, dashState, new FuncPredicate(() => _isDashBuffered && CanDash()));
            At(jumpState, attackState, new FuncPredicate(() => playerCombat.IsAttacking));
            At(jumpState, locomotionState, new FuncPredicate(() => groundChecker.IsGrounded && rb.linearVelocity.y <= 0.01f));

            At(dashState, locomotionState, new FuncPredicate(() => !DashTimer.IsRunning && groundChecker.IsGrounded));
            At(dashState, jumpState, new FuncPredicate(() => !DashTimer.IsRunning && !groundChecker.IsGrounded));

            At(attackState, locomotionState, new FuncPredicate(() => !playerCombat.IsAttacking && groundChecker.IsGrounded));
            At(attackState, jumpState, new FuncPredicate(() => !playerCombat.IsAttacking && !groundChecker.IsGrounded));

            _stateMachine.SetState(locomotionState);
        }
        void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);


        // --- Input Handling (Adapted for YOUR InputReader) ---
        private void OnMove(Vector2 rawInput) => MovementInput = new Vector3(rawInput.x, 0, rawInput.y);
        private void OnJumpInput(bool performed)
        {
            if (performed) JumpBufferTimer.Start();
            else { if (JumpHoldTimer.IsRunning && rb.linearVelocity.y > 0) JumpHoldTimer.Stop(); }
        }
        private void OnDashInput(bool performed)
        {
            if (performed) _isDashBuffered = true;
        }
        private void OnLightAttack() => playerCombat?.ReceiveLightAttackInput();
        private void OnHeavyAttack() => playerCombat?.ReceiveHeavyAttackInput();


        // --- Action Conditions ---
        private bool CanJump() => JumpBufferTimer.IsRunning && (groundChecker.IsGrounded || CoyoteTimer.IsRunning) && !JumpCooldownTimer.IsRunning;
        private bool CanDash() => !DashTimer.IsRunning && !DashCooldownTimer.IsRunning;


        // --- Physics & Movement Methods (Called by States) ---
        public void HandleMovementAndRotation()
        {
            if (_mainCamTransform == null) return;
            Vector3 camForward = Vector3.Scale(_mainCamTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(_mainCamTransform.right, new Vector3(1, 0, 1)).normalized;
            _targetMoveDirection = (camForward * MovementInput.z + camRight * MovementInput.x).normalized;

            float targetSpeed = MovementInput.magnitude * moveSpeed;
            _currentHorizontalSpeed = Mathf.SmoothDamp(_currentHorizontalSpeed, targetSpeed, ref _speedSmoothVelocity, movementSmoothTime);

            if (_targetMoveDirection.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_targetMoveDirection);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                Vector3 newHorizontalVelocity = _targetMoveDirection * _currentHorizontalSpeed;
                rb.linearVelocity = new Vector3(newHorizontalVelocity.x, rb.linearVelocity.y, newHorizontalVelocity.z);
            }
            else
            {
                _currentHorizontalSpeed = Mathf.SmoothDamp(_currentHorizontalSpeed, 0, ref _speedSmoothVelocity, movementSmoothTime);
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        public void HandleGravity()
        {
            float gravityScale = gravityMultiplier;
            if (!JumpHoldTimer.IsRunning && rb.linearVelocity.y > 0)
            {
                gravityScale *= 2f;
            }
            Vector3 gravity = Physics.gravity * gravityScale * Time.fixedDeltaTime;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        public void PerformJump()
        {
            JumpBufferTimer.Stop();
            CoyoteTimer.Stop();
            JumpHoldTimer.Start();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(AnimJumpTrigger);
        }

        public void PerformDash()
        {
            _isDashBuffered = false;
            DashTimer.Start();
            animator.SetTrigger(AnimDashTrigger);
            Vector3 dashDir = _targetMoveDirection.magnitude > 0.1f ? _targetMoveDirection : transform.forward;
            rb.linearVelocity = new Vector3(dashDir.x * dashForce, 0f, dashDir.z * dashForce);
        }

        private void StartCoyoteTimer() { if (!groundChecker.IsGrounded) CoyoteTimer.Start(); }

        void UpdateAnimator()
        {
            float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
            animator.SetFloat(AnimSpeed, horizontalSpeed);
            animator.SetBool(AnimIsGrounded, groundChecker.IsGrounded);
        }
    }
}
