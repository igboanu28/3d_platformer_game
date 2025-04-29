using UnityEngine;
using TMPro;


namespace Platformer
{
    public class QLogButtonScript : MonoBehaviour
    {
        public string questID;
        public TextMeshProUGUI questTitle;

        public void ShowAllInfos()
        {
            // Ensure QuestManager and QuestUIManager are available
            if (QuestManager.Instance == null || QuestUIManager.Instance == null)
            {
                Debug.LogError("QuestManager or QuestUIManager instance is not available.");
                return;
            }

            // Retrieve the quest by ID from QuestManager
            QuestData quest = QuestManager.Instance.GetQuestByID(questID);

            if (quest == null)
            {
                Debug.LogWarning($"Quest with ID {questID} not found in QuestManager.");
                return;
            }

            // Handle quests that are in progress or complete
            if (quest.progress == QuestProgress.ACCEPTED || quest.progress == QuestProgress.COMPLETE)
            {
                QuestUIManager.Instance.ShowQuestInProgress(quest); // Call ShowQuestInProgress from QuestUIManager
            }
            else
            {
                QuestUIManager.Instance.ShowQuestLog(quest); // For other states, use the default ShowQuestLog method
            }

        }

        public void ClosePanel()
        { 
            QuestUIManager.Instance.HideQuestLogPanel();

        }
    }
}
