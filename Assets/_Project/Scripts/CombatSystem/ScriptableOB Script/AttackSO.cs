using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/Combat/AttackSO")]
    public class AttackSO : ScriptableObject
    {
        [Header("Information")] // Attack information
        public string attackName = "Unnamed Attack";
        public AttackInputType inputType = AttackInputType.Light;
        [Tooltip("The trigger name in the Animator Controller")]
        public string animationTriggerName;
        public float damage = 10;

        [Header("Hitbox Properties")]
        [Tooltip("Radius of the OverlapSphere for hit detection.")]
        public float hitboxRadius = 0.75f;
        [Tooltip("Offset of the hitbox center from the 'HitPointOrigin' transform, relative to player's forward direction")]
        public Vector3 hitboxOffset = new Vector3(0, 0.5f, 0.75f);

        [Header("Timing & Flow (Controlled by Animation Events)")]
        [Tooltip("Maximum duration this attack will be considered active if animation events fail. Serves as a failsafe.")]
        public float maxAttackDuration = 1.0f;

        [Header("Movement & Control")]
        public bool requiresGrounded = true; // This helps check if player can only perform grounded attack while grounded
        [Tooltip("Applies a small forward impulse when the attack starts.")]
        public float forwardMomentum = 0f;
        // public bool allowMovementDuringAttack = false; // For now no but for future consideration

        [Header("Feedback System")]
        public GameObject hitVFXPrefab; // Particle effect instantiated on successful hit
        public AudioClip swingSFX; // sound played when the attack is initiated
        public AudioClip hitSFX; // Sound played on successful hit
        [Tooltip("Duration for 'hit stop' effect (brief time pause) on successful hit. Set to 0 for no hit stop.")]
        public float hitStopDuration = 0.05f;

        [Header("Combo Chaing")]
        [Tooltip("The next AttackData to chain into if a Light attack input is buffered.")]
        public AttackSO nextLightAttackInCombo;
        [Tooltip("The next AttackData to chain into if a Heavy attack input is buffered.")]
        public AttackSO nextHeavyAttackInCombo;
        // in the future i can add more complex combo paths, e.g., AttackData specificToButtonSequence;

    }

    public enum AttackInputType 
    { 
        Light, 
        Heavy
    }
}
