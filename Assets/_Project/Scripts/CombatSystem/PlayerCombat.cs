using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Platformer;

namespace CombatSystem
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Component References")]
        [Tooltip("Reference to the main PlayerController script for state and ground checks")]
        [SerializeField] private PlayerController playerController;
        [Tooltip("Animator component on this GameObject")]
        [SerializeField] private Animator animator;
        [Tooltip("Optional: An empty child GameObject marking the origin for hitbox cast. if null, this transform is used.")]
        [SerializeField] private Transform hitPointOrigin;

        [Header("Default Attack Assignments")]
        [Tooltip("AttackData for the initial ground light attack.")]
        [SerializeField] private AttackSO initialGroundLightAttack;
        [Tooltip("AttackData for the initial aerial light attack.")]
        [SerializeField] private AttackSO initialAerialLightAttack;
        [Tooltip("AttackData for the initial ground heavy attack")]
        [SerializeField] private AttackSO initialGroundHeavyAttack;
        [Tooltip("AttackData for the initial aerial heavy attack.")]
        [SerializeField] private AttackSO initialAerialHeavyAttack;

        [Header("Combat Setting")]
        [Tooltip("Layer mask defining what entities are considered 'Enemy'.")]
        [SerializeField] private LayerMask enemyLayer;
        [Tooltip("How long an attack input is buffered if pressed during another attack.")]
        [SerializeField] private float attackBufferTime = 0.3f;

        // Public property for the PlayerController's state machine 
        public bool IsAttacking { get; private set; }
        private AttackSO _currentAttackData; //For states or UI to query

        private AttackSO _bufferedAttackData; // Stores the next attack if input is buffered
        private float _attackBufferTimer; // Countdown for the input buffer
        private float _currentAttackFailsafeTimer; // Failsafe for ending stuck attack

        void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (playerController == null) playerController = GetComponentInParent<Platformer.PlayerController>(); // Or GetComponent for same GameObject
            if (playerController == null) Debug.LogError("PlayerController not found by PlayerCombat!", this);
        }

        void Update()
        {
            // Tick down the input buffer timer
            if (_attackBufferTimer > 0)
            {
                _attackBufferTimer -= Time.deltaTime;
                if (_attackBufferTimer <= 0)
                {
                    _bufferedAttackData = null; // Clear
                }
            }

            // Failsafe timer for current attack
            if (IsAttacking && _currentAttackData != null)
            {
                _currentAttackFailsafeTimer -= Time.deltaTime;
                if (_currentAttackFailsafeTimer <= 0)
                {
                    Debug.LogWarning($"Attack '{_currentAttackData.attackName}' time out (failsafe). Forcing end.");
                    ForceEndAttackSequence();
                }
            }
        }

        // Called by PlayerController when a light attack input is detected
        public void ReceiveLightAttackInput()
        {
            if (playerController == null) return;
            AttackSO attackToAttempt = playerController.GroundChecker.IsGrounded ? initialGroundLightAttack : initialAerialLightAttack;
            ProcessAttackInput(attackToAttempt, AttackInputType.Light);
        }

        // Called by PlayerController when a heavy attack input is detected
        public void ReceiveHeavyAttackInput()
        {
            if (playerController == null) return;
            AttackSO attackToAttempt = playerController.GroundChecker.IsGrounded ? initialGroundHeavyAttack : initialAerialHeavyAttack;
            ProcessAttackInput(attackToAttempt, AttackInputType.Heavy);
        }

        private void ProcessAttackInput(AttackSO attackToAttempt, AttackInputType inputType)
        {
            if (attackToAttempt == null)
            {
                Debug.Log($"No {inputType} attack defined for current state (Grounded: {playerController.GroundChecker.IsGrounded}).");
                return;
            }

            if (IsAttacking && _currentAttackData != null) // If already attacking, try to buffer for a combo
            {
                AttackSO nextCombo = (inputType == AttackInputType.Light) ? _currentAttackData.nextLightAttackInCombo : _currentAttackData.nextHeavyAttackInCombo;

                if (nextCombo != null) // Check if the current attack *can* combo into this input type
                {
                    // Only buffer if the attack being attempted matches the defined combo path
                    if (attackToAttempt == nextCombo || (attackToAttempt.inputType == inputType && (playerController.GroundChecker.IsGrounded != nextCombo.requiresGrounded && !nextCombo.requiresGrounded)))
                    {
                        _bufferedAttackData = nextCombo;
                        _attackBufferTimer = attackBufferTime;
                    }
                    else if (attackToAttempt.inputType == inputType) // if it's a generic light/heavy and the current attack has a generic light/heavy combo
                    {
                        _bufferedAttackData = nextCombo;
                        _attackBufferTimer = attackBufferTime;
                    }
                    else
                    {
                        _bufferedAttackData = null; // Not a valid combo continuation from current attack
                        Debug.Log("Input does not match valid combo path from " + _currentAttackData.name);
                    }
                    if (_bufferedAttackData != null)
                    {
                        _attackBufferTimer = attackBufferTime;
                        Debug.Log($"Buffered Combo: {_bufferedAttackData.attackName}");
                    }
                }
                // else: Current attack has no combo defined for this input type. Input ignored for combo.
            }
            else // not currently attacking, eligible to start a new attack sequence
            {
                if (attackToAttempt.requiresGrounded && !playerController.GroundChecker.IsGrounded)
                {
                    Debug.Log($"Cannot perform '{attackToAttempt.attackName}', requires grounded.");
                    return;
                }
                StartAttackSequence(attackToAttempt);
            }
        }

        private void StartAttackSequence(AttackSO attackSO)
        { 
            _currentAttackData = attackSO;
            IsAttacking = true; // PlayerController's StateMachine uses this
            _currentAttackFailsafeTimer = _currentAttackData.maxAttackDuration;

            animator.SetTrigger(_currentAttackData.animationTriggerName);
            // Debug.Log($"Starting Attack: {_currentAttackData.attackName} (Anim: {_currentAttackData.animationTriggerName})");

            if (FeedbackManager.Instance != null && _currentAttackData.swingSFX != null)
            {
                FeedbackManager.Instance.PlaySFX(_currentAttackData.swingSFX, transform.position);
            }

            if (_currentAttackData.forwardMomentum > 0 && playerController.Rb != null)
            {
                // Apply momentum relative to player's facing directiom
                playerController.Rb.AddForce(transform.forward * _currentAttackData.forwardMomentum, ForceMode.Impulse);
            }

            // Clear any consumed buffer
            _bufferedAttackData = null;
            _attackBufferTimer = 0f;
        }

        private void ForceEndAttackSequence()
        {
            if (!IsAttacking) return;
            Debug.Log("Force ending attack sequence.");
            IsAttacking = false;
            _currentAttackData = null;
            _bufferedAttackData = null;
            _attackBufferTimer = 0f;
            //animator.CrossFade("Idle", 0.1f); // Or your locomotion state

            // Optionally, force animator to a know safe state if it gets stuck
            //animaor.CrossFade("Idle", 0.1f); // Replace "Idle" with your idle state name
        }

        // --- ANIMATION EVENT METHODS ---
        // These public methods MUST be called by Animation Events in your attack animation clips.

        public void AnimationEvent_PerformHitDetection()
        {
            if (!IsAttacking || _currentAttackData == null) return;

            Debug.Log($"Hit Detection Triggered for: {_currentAttackData.attackName}");
            Vector3 castOrigin = hitPointOrigin != null ? hitPointOrigin.position : transform.position;
            Vector3 worldOffset = transform.TransformDirection(_currentAttackData.hitboxOffset);
            Vector3 hitboxCenter = castOrigin + worldOffset;

            Collider[] hitColliders = Physics.OverlapSphere(hitboxCenter, _currentAttackData.hitboxRadius, enemyLayer, QueryTriggerInteraction.Ignore);

            foreach (var hitCollider in hitColliders)
            { 
                if (hitCollider.gameObject == playerController.gameObject) continue; // Ignore hitting self

                if (hitCollider.TryGetComponent(out Health enemyHealth))
                { 
                    enemyHealth.TakeDamage(_currentAttackData.damage, _currentAttackData); // Pass AttackSO for context additional effects

                    if (FeedbackManager.Instance != null)
                    { 
                        if (_currentAttackData.hitVFXPrefab != null)
                            FeedbackManager.Instance.PlayVFX(_currentAttackData.hitVFXPrefab, hitCollider.ClosestPoint(hitboxCenter));
                        if (_currentAttackData.hitSFX != null)
                            FeedbackManager.Instance.PlaySFX(_currentAttackData.hitSFX, hitCollider.transform.position);
                    }

                    if (_currentAttackData.hitStopDuration > 0)
                    { 
                        
                    }
                    Debug.Log($"Hit: {hitCollider.name} with {_currentAttackData.attackName}");

                }
            }
        }

        public void AnimationEvent_AllowCombo()
        {
            if (!IsAttacking || _currentAttackData == null) return;

            if (_bufferedAttackData != null && _attackBufferTimer > 0)
            {
                Debug.Log($"Combo Success: Chaining into '{_bufferedAttackData.attackName}' from '{_currentAttackData.attackName}'.");
                StartAttackSequence(_bufferedAttackData); // _bufferedAttackData is cleared in StartAttackSequence
            }
        }

        public void AnimationEvent_AttackEnd()
        {
            if (_bufferedAttackData == null && _attackBufferTimer <= 0) // if no combo is pending
            {
                Debug.Log($"Animation End for '{_currentAttackData?.attackName}'. Setting IsAttacking to false.");
                IsAttacking = false;
                _currentAttackData = null; // Clear current attack
            }
            // If a combo *did* happen, IsAttacking is already true for the new attack, and _currentAttackData points to the new one.
            // PlayerController's state machine will then handle transitioning out of its "AttackState" when IsAttacking becomes false.
        }

        void OnDrawGizmosSelected()
        {
            // Visualize the hitbox for easier setup if an AttackData is active or a default one is assigned
            AttackSO dataToVisualize = null;
            if (IsAttacking && _currentAttackData != null)
            {
                dataToVisualize = _currentAttackData;
                Gizmos.color = Color.red; // Active attack
            }
            else if (initialGroundLightAttack != null)
            {
                dataToVisualize = initialGroundLightAttack; // Default visualization
                Gizmos.color = Color.gray;
            }

            if (dataToVisualize != null)
            {
                Vector3 castOrigin = hitPointOrigin != null ? hitPointOrigin.position : transform.position;
                Vector3 worldOffset = transform.TransformDirection(dataToVisualize.hitboxOffset);
                Gizmos.DrawWireSphere(castOrigin + worldOffset, dataToVisualize.hitboxRadius);
            }
        }
    }
}