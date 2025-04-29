using UnityEngine;

namespace Platformer
{
    public class NPCQuestHandler : MonoBehaviour
    {
        public string npcID;
        public QuestData assignedQuest; // Quest assigned by this NPC

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartDialogue();
            }
        }

        void StartDialogue()
        {
            if (assignedQuest == null)
            {
                Debug.LogWarning($"NPC {npcID} does not have an assigned quest.");
                return;
            }
            Debug.Log($"NPC Dialogue: {assignedQuest.description}");
            // Call the UI manager to show the quest details without automatically accepting the quest
            QuestUIManager.Instance.ShowQuestAvailable(assignedQuest);
            QuestManager.Instance.AcceptQuest(assignedQuest); // Accept quest
        }

        public void AdvanceQuestStage()
        {
            if (assignedQuest.linkedNPCIDs.Contains(npcID) &&
                assignedQuest.currentStage == assignedQuest.linkedNPCIDs.IndexOf(npcID))
            {
                assignedQuest.AdvanceStage();
                Debug.Log($"Quest stage advanced: {assignedQuest.stageDescriptions[assignedQuest.currentStage]}");
            }
        }
    }
}