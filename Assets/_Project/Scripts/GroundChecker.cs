using UnityEngine;

namespace Platformer
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

        /* 
         You will need to create a event channel to notify when the player is grounded or not.
        no need to create a separate event for when the player is not grounded, like the boolevent or so just do this (Right-click -> Create -> Events -> EventChannel).
         */
        [Header("Event Channels")]
        [Tooltip ("The EventChannel to invoke when the player became grounded.")]
        [SerializeField] private EventChannel onGroundedEventChannel; // For Player Landed event
        [Tooltip("The EventChannel to invoke when the player leaves the ground.")]
        [SerializeField] private EventChannel onLeftGroundEventChannel; // For Player Left Ground event

        // Stores the current qrounded status
        public bool IsGrounded { get; private set; }
        
        // Stores the previous grounded status
        private bool _wasGrounded;

        private void Awake()
        {
            // Initialize _wasGrounded based on initial check
            // Perform an initial check to set IsGrounded correctly
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);
            _wasGrounded = IsGrounded;
        }

        void Update()
        {

            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);

            if (IsGrounded && !_wasGrounded)
            {
                onGroundedEventChannel?.Invoke(new Empty()); // Invoke the ScriptableObject event
                Debug.Log("GroundChecker: Player is now grounded!");
            }
            else if (!IsGrounded && _wasGrounded)
            {
                onLeftGroundEventChannel?.Invoke(new Empty()); // Invoke the ScriptableObject event
                Debug.Log("GroundChecker: Player has left the ground!");
            }

            // Update _wasGrounded for the next frame
            _wasGrounded = IsGrounded;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            // Assuming the origin for the sphere cast is at the transform's position
            // and it casts downwards by groundDistance. The sphere itself is at the origin.
            Gizmos.DrawWireSphere(transform.position, groundDistance);
            // If you want to visualize the end point of the cast:
            // Gizmos.DrawWireSphere(transform.position + Vector3.down * groundDistance, groundDistance);
        }
    }
}