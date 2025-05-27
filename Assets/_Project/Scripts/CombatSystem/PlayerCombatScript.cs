using Enemy;
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
        public int attackDamage;

        public Transform attackPoint;
        public float attackRange = 0.5f;
        public float attackRate = 2f; // Attacks per second
        public LayerMask enemyLayer;
        
        private float nextAttackTime = 0f;

        private int comboStep = 0;
        private float lastAttackTime = 0f;
        public float comboResetTime = 1f; // Time to reset combo if no attacks are made

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
            if (Time.time >= nextAttackTime)
            {
                if (Time.time - lastAttackTime > comboResetTime)
                {
                    comboStep = 0; // Reset combo if too much time has passed
                }

                comboStep++;
                lastAttackTime = Time.time;
                nextAttackTime = Time.time + 1f / attackRate; // Calculate the next attack time

                switch (comboStep)
                {
                    case 1:
                        animator.SetTrigger("Attack");
                        EnemyDamage(10);
                        break;

                    case 2:
                        animator.SetTrigger("Attack2");
                        EnemyDamage(15);
                        break;

                    case 3:
                        animator.SetTrigger("Attack3");
                        EnemyDamage(20);
                        comboStep = 0; // Reset combo after third attack
                        break;

                    default:
                        comboStep = 0; // Reset combo if it exceeds 3
                        break;
                }
            }
        }

        void OnSpinAttack()
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Spin_attack");
                nextAttackTime = Time.time + 1f / attackRate; // Calculate the next attack time

                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    Debug.Log("Spin attack");
                    enemy.GetComponent<Health>().TakeDamage(attackDamage = (int)20f);
                    Debug.Log("Spin damage");
                }
            }
        }

        void OnHeavyAttack()
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Dual_Weapon");
                nextAttackTime = Time.time + 1f / attackRate; // Calculate the next attack time

                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    Debug.Log("Heavy attack");
                    enemy.GetComponent<Health>().TakeDamage(attackDamage = (int)30f);
                    Debug.Log("Heavy damage");
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        void EnemyDamage(int attackDamage)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("Light attack");
                var health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(attackDamage);
                    Debug.Log("light damage");
                }
            }
        }
    }
}