using CombatSystem;
using UnityEngine;

namespace Platformer
{
    public class AttackState : IState
    {
        private readonly PlayerController _player;
        private readonly PlayerCombat _playerCombat;
        public AttackState(PlayerController player, PlayerCombat playerCombat)
        {
            _player = player;
            _playerCombat = playerCombat;

        }

        public void OnEnter()
        {
            
        }

        public void FixedUpdate()
        {
        }

    }
}
