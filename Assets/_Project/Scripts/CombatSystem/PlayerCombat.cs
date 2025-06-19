using KBCore.Refs;
using Platformer;
using UnityEngine;
using System.Collections;

namespace CombatSystem
{
    public partial class PlayerCombat : MonoBehaviour
    {

        private Animator animator;
        [SerializeField, Anywhere] private InputReader input;
        // Add this to your PlayerCombat script's variable declarations
        [SerializeField] private GameObject weaponHitbox;

        [Header("Combo Settings")]
        private bool activateTimerToReset;
        private float default_Combo_Timer = 0.4f;
        private float current_Combo_Timer;
        private ComboState current_Combo_State;

        // this is to trigger the attack animations
        private const string ATTACK_1_TRIGGER = "Attack1";
        private const string ATTACK_2_TRIGGER = "Attack2";
        private const string ATTACK_3_TRIGGER = "Attack3";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (weaponHitbox != null)
            {
                weaponHitbox.SetActive(false); // Make sure the hitbox is off at the start
            }
        }

        void Start()
        {
            current_Combo_Timer = default_Combo_Timer;
            current_Combo_State = ComboState.NONE;
        }

        private void OnEnable()
        {
            input.LightAttack += ComboAttacks;
        }

        private void OnDisable()
        {
            input.LightAttack -= ComboAttacks;
        }

        public void Combo1()
        {
            animator.SetTrigger(ATTACK_1_TRIGGER);
        }
        
        public void Combo2()
        {
            animator.SetTrigger(ATTACK_2_TRIGGER);
        }

        public void Combo3()
        {
            animator.SetTrigger(ATTACK_3_TRIGGER);
        }

        void Update()
        {
            //ComboAttacks();
            ResetComboState();
        }

        void ComboAttacks()
        {
            if (current_Combo_State == ComboState.Combo3) return; // Prevent further combos if already at Combo3

            current_Combo_State++;
            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            // Activate the hitbox for a short duration
            StartCoroutine(ActivateHitboxForDuration(0.3f)); // Adjust the duration as needed


            if (current_Combo_State == ComboState.Combo1)
                {
                    Combo1();
                }
                else if (current_Combo_State == ComboState.Combo2)
                {
                    Combo2();
                }
                else if (current_Combo_State == ComboState.Combo3)
                {
                    Combo3();
                }
        }

        void ResetComboState()
        {
            if (activateTimerToReset)
            {
                current_Combo_Timer -= Time.deltaTime;
                if (current_Combo_Timer <= 0f)
                {
                    current_Combo_State = ComboState.NONE;
                    activateTimerToReset = false;
                    current_Combo_Timer = default_Combo_Timer; // Reset the timer for the next combo
                    DisableWeaponHitbox();

                }
            }
        }

        IEnumerator ActivateHitboxForDuration(float duration)
        {
            EnableWeaponHitbox();
            yield return new WaitForSeconds(duration);
            DisableWeaponHitbox();
        }

        public void EnableWeaponHitbox()
        {
            if (weaponHitbox != null)
            {
                weaponHitbox.SetActive(true);
            }
        }

        public void DisableWeaponHitbox()
        {
            if (weaponHitbox != null)
            {
                weaponHitbox.SetActive(false);
            }
        }
    }

    public enum ComboState
    {
        NONE,
        Combo1,
        Combo2,
        Combo3
    }
}