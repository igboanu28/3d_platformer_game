//using UnityEngine;

//namespace Platformer
//{
//    public class AttackState : BaseState
//    {
//        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

//        public override void OnEnter()
//        {
//            //animator.CrossFade(AttackHash, crossFadeDuration);
//            //player.Attack();
//        }

//        public override void FixedUpdate()
//        {
//            player.HandleMovement();
//        }
//    }
//}


//using UnityEngine;

//namespace CombatSystem
//{
//    public class PlayerCombat : MonoBehaviour
//    {
//        // Animation trigger names (formerly in TagManager/AnimationTags)
//        private const string ATTACK_1_TRIGGER = "Attack1";
//        private const string ATTACK_2_TRIGGER = "Attack2";
//        private const string ATTACK_3_TRIGGER = "Attack3";

//        [System.Serializable]
//        public struct Attack
//        {
//            public string animationTriggerName;
//            public float attackDuration;
//        }

//        [SerializeField] private Attack[] comboAttacks;
//        [SerializeField] private float comboResetTime = 1.0f;

//        private int currentAttackIndex = 0;
//        private float lastAttackTime = 0f;
//        private bool isAttacking = false;
//        private bool comboQueued = false;
//        private Animator animator;

//        public bool IsAttacking => isAttacking;

//        void Awake()
//        {
//            animator = GetComponent<Animator>();
//        }

//        void Update()
//        {
//            if (Input.GetButtonDown("LightAttack"))
//            {
//                if (!isAttacking)
//                {
//                    StartAttack();
//                }
//                else
//                {
//                    comboQueued = true;
//                }
//            }

//            if (isAttacking && Time.time - lastAttackTime > comboAttacks[currentAttackIndex].attackDuration + comboResetTime)
//            {
//                ResetCombo();
//            }
//        }

//        private void StartAttack()
//        {
//            isAttacking = true;
//            currentAttackIndex = 0;
//            PlayAttackAnimation();
//        }

//        private void PlayAttackAnimation()
//        {
//            var attack = comboAttacks[currentAttackIndex];
//            // Use the trigger name from the struct, or use the constants above if you prefer
//            animator.SetTrigger(attack.animationTriggerName);
//            lastAttackTime = Time.time;
//            comboQueued = false;
//        }

//        public void OnAttackAnimationEnd()
//        {
//            if (comboQueued && currentAttackIndex < comboAttacks.Length - 1)
//            {
//                currentAttackIndex++;
//                PlayAttackAnimation();
//            }
//            else
//            {
//                ResetCombo();
//            }
//        }

//        private void ResetCombo()
//        {
//            isAttacking = false;
//            comboQueued = false;
//            currentAttackIndex = 0;
//        }
//    }
//}






//if (Input.GetButtonDown("HeavyAttack"))
//{
//    if (current_Combo_State == ComboState.Aerial_Heavy_Attack) return; // Prevent further aerial heavy attacks if already in that state
//    current_Combo_State = ComboState.Aerial_Heavy_Attack;
//    activateTimerToReset = true;
//    current_Combo_Timer = default_Combo_Timer;
//    // Trigger the aerial heavy attack animation
//    animator.SetTrigger("AerialHeavyAttack");
//}