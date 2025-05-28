using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/Combat/AttackSO")]
    public class AttackSO : ScriptableObject
    {
        public AnimatorOverrideController animatorOverrideController;
        public int damage;
    }
}
