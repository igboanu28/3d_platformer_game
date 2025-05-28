using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace CombatSystem
{
    public class PlayerCombat : MonoBehaviour
    {
        public List<AttackSO> combo;
        float lastClickedTime;
        float lastComboEnd;
        int combotStep;
        Animator animator;

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        void OnEnable()
        {
            // Subscribe to input events if using an InputReader
            // inputReader.LightAttack += OnLightAttack;
            // inputReader.SpinAttack += OnSpinAttack;
            // inputReader.HeavyAttack += OnHeavyAttack;
        }
        void OnDisable()
        {
            // Unsubscribe from input events
            // inputReader.LightAttack -= OnLightAttack;
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
                
                }
            }
        }

        void ExitLightAttack()
        { 
        
        }

        void EndCombo()
        { 
        
        }
    }
}
