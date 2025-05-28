using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Platformer;

namespace CombatSystem
{
    public class PlayerCombat : MonoBehaviour
    {
        public List<AttackSO> combo;
        float lastClickedTime;
        float lastComboEnd;
        int combotStep;
        Animator animator;
        Weapon weapon;
        [SerializeField] InputReader inputReader;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            ExitLightAttack();
        }

        void OnEnable()
        {
            // Subscribe to input events if using an InputReader
             inputReader.LightAttack += OnlightAttack;
            // inputReader.SpinAttack += OnSpinAttack;
            // inputReader.HeavyAttack += OnHeavyAttack;
        }
        void OnDisable()
        {
            // Unsubscribe from input events
            inputReader.LightAttack -= OnlightAttack;
            // inputReader.SpinAttack -= OnSpinAttack;
            // inputReader.HeavyAttack -= OnHeavyAttack;
        }
        void OnlightAttack()
        {
            if (Time.time - lastComboEnd > 0.5f && combotStep <= combo.Count)
            {
                CancelInvoke("EndCombo");
                if (Time.time - lastClickedTime >= 0.2f)
                { 
                    animator.runtimeAnimatorController = combo[combotStep].animatorOverrideController;
                    animator.Play("LightAttack", 0, 0);
                    weapon.damage = combo[combotStep].damage;
                    combotStep++;
                    lastClickedTime = Time.time;

                    if (combotStep > combo.Count)
                    {
                        combotStep = 0; // Reset combo step if it exceeds the count
                    }
                }
            }
        }

        void ExitLightAttack()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && animator.GetCurrentAnimatorStateInfo(0).IsTag("LightAttack"))
            { 
                Invoke("EndCombo", 1); // Delay to allow the animation to finish
            }
        }

        void EndCombo()
        {
            combotStep = 0; // Reset combo step
            lastComboEnd = Time.time; // Record the end time of the combo
        }
    }
    
}
