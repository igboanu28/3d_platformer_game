using UnityEngine;
using Platformer;

namespace CombatSystem
{
    [CreateAssetMenu(menuName = "Events/AttackDataEventChannel", fileName = "NewAttackDataEventChannel")]
    public class AttackDataEventChannel : EventChannel<AttackSO>
    {
        // Empty, inherits from EventChannel<AttackData>
    }
}
