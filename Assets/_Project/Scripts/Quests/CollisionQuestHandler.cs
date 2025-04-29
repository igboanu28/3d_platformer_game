using UnityEngine;
using System.Collections;

namespace Platformer
{
    public class CollisionQuestHandler : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                QuestManager.Instance.AddQuestItem("Reached", 1);
            }

        }
    }
        
}
