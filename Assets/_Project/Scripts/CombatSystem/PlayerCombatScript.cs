using KBCore.Refs;
using Platformer;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerCombatScript : MonoBehaviour
    {
        [Header("Combat Setup")]
        [SerializeField] Animator animator;
        [SerializeField] InputReader inputReader;

        public Transform attackPoint;
        private float attackRange = 0.5f;

        public LayerMask enemyLayer;

        private void OnEnable()
        {
            inputReader.LightAttack += OnLightAttack;
            inputReader.SpinAttack += OnSpinAttack;
            inputReader.HeavyAttack += OnHeavyAttack;
        }

        private void OnDisable()
        {
            inputReader.LightAttack -= OnLightAttack;
            inputReader.SpinAttack -= OnSpinAttack;
            inputReader.HeavyAttack -= OnHeavyAttack;
        }

        void OnLightAttack()
        { 
            animator.SetTrigger("Attack");


            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider collider in hitEnemies)
            {
                Debug.Log("Light attack");
            }
        }

        void OnSpinAttack()
        {
            animator.SetTrigger("Attack");


            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider collider in hitEnemies)
            {
                Debug.Log("Spin attack");
            }
        }

        void OnHeavyAttack()
        {
            animator.SetTrigger("Attack");


            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider collider in hitEnemies)
            {
                Debug.Log("Heavy attack");
            }
        }
    }
}