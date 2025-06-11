using DialogueSystem;
using UnityEngine;

namespace Platformer
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Ground Check Settings")]
        [Tooltip("Position of the ground check sphere, relative to this GameObject's transform.")]
        [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -0.9f, 0); // Adjust based on character pivot
        [Tooltip("Radius of the sphere used for ground checking.")]
        [SerializeField] private float groundCheckRadius = 0.25f;
        [Tooltip("Layers considered as 'ground'.")]
        [SerializeField] private LayerMask groundLayer;

        // NEW: References to the ScriptableObject EventChannel assets
        [Header("Event Channels (Out)")]
        [Tooltip("The EventChannel to invoke when the player becomes grounded.")]
        [SerializeField] private BoolEventChannel onGroundedEventChannel; // This is your Platformer.EventChannel (EventChannel<Empty>)
        [Tooltip("The EventChannel to invoke when the player leaves the ground.")]
        [SerializeField] private BoolEventChannel onLeftGroundEventChannel; // This is your Platformer.EventChannel (EventChannel<Empty>)


        public bool IsGrounded { get; private set; }
        public RaycastHit GroundHitInfo { get; private set; } // Information about what was hit

        private bool _wasGroundedLastFrame;

        void Awake()
        {
            // Initialize _isGroundedLastFrame based on initial check
            // Perform an initial check to set IsGrounded correctly
            Vector3 checkPosition = transform.TransformPoint(groundCheckOffset);
            _wasGroundedLastFrame = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
            IsGrounded = _wasGroundedLastFrame; // Set initial IsGrounded state
        }


        void FixedUpdate() // Best to do physics checks in FixedUpdate
        {
            PerformGroundCheck();
        }

        private void PerformGroundCheck()
        {
            Vector3 checkPosition = transform.TransformPoint(groundCheckOffset); // Convert local offset to world position

            // Perform an OverlapSphere or SphereCast. OverlapSphere is often sufficient.
            IsGrounded = Physics.CheckSphere(checkPosition, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

            // Optional: Get detailed hit info with SphereCast
            // if (Physics.SphereCast(transform.position + Vector3.up * groundCheckRadius, groundCheckRadius, Vector3.down, out RaycastHit hit, 0.2f + groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore))
            // {
            //     IsGrounded = true;
            //     GroundHitInfo = hit;
            // }
            // else
            // {
            //     IsGrounded = false;
            // }

            // Handle landing/leaving ground events
            if (!_wasGroundedLastFrame && IsGrounded)
            {
                onGroundedEventChannel?.Invoke(true); // Invoke the ScriptableObject event
                Debug.Log("GroundChecker: Player is now grounded!");
            }
            else if (_wasGroundedLastFrame && !IsGrounded)
            {
                onLeftGroundEventChannel?.Invoke(false); // Invoke the ScriptableObject event
                Debug.Log("GroundChecker: Player just left the ground!");
            }
            _wasGroundedLastFrame = IsGrounded; // Update for the next frame's comparison
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            if (Application.isPlaying) Gizmos.color = IsGrounded ? Color.green : Color.red;
            else Gizmos.color = Color.yellow; // Yellow in editor when not playing

            Vector3 checkPosition = transform.TransformPoint(groundCheckOffset);
            Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
        }
    }
}