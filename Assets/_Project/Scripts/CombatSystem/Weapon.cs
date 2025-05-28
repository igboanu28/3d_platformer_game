using UnityEngine;
using Platformer;

namespace CombatSystem
{
    public class Weapon : MonoBehaviour
    {
        public float damage;

        BoxCollider triggerBox;

        private void Start()
        {
            triggerBox = GetComponent<BoxCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider has a Health component (i.e., is an enemy)
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage((int)damage);

            }
        }

        public void EnableTriggerBox()
        {
            if (triggerBox != null)
            {
                triggerBox.enabled = true;
            }
        }

        public void DisableTriggerBox()
        {
            if (triggerBox != null)
            {
                triggerBox.enabled = false;
            }
        }
    }
    
}
