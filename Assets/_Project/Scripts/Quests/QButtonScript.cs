using UnityEngine;
using TMPro;
using UnityEditor;


namespace Platformer
{
    public class QButtonScript : MonoBehaviour
    {
        public string questID;
        public TextMeshProUGUI questTitle;

        private GameObject acceptButton;
        private GameObject giveUpButton;
        private GameObject completeButton;

        void Start()
        {
            //acceptButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("AcceptButton").gameObject;
            //acceptButtonScript = acceptButton.GetComponent<QButtonScript>();

            //giveUpButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("GiveUpButton").gameObject;
            //giveUpButtonScript = giveUpButton.GetComponent<QButtonScript>();

            //completeButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("CompleteButton").gameObject;
            //completeButtonScript = completeButton.GetComponent<QButtonScript>();

            //Cache references to buttons
            acceptButton = FindButton("AcceptButton");
            giveUpButton = FindButton("GiveUpButton");
            completeButton = FindButton("CompleteButton");

            // Ensure buttons are initially inactive
            if (acceptButton != null) acceptButton.SetActive(false);
            if (giveUpButton != null) giveUpButton.SetActive(false);
            if (completeButton != null) completeButton.SetActive(false);
        }
        
        // Helper method to find buttons
        private GameObject FindButton(string buttonName)
        {
            Transform buttonTransform = GameObject.Find("QuestCanvas")
                ?.transform.Find("QuestPanel/QuestDescription/GameObject/" + buttonName);

            if (buttonTransform == null)
            {
                Debug.LogWarning($"{buttonName} not found in the QuestCanvas hierarchy.");
                return null;
            }

            return buttonTransform.gameObject;
        }

        // SHOW ALL INFOS
        public void ShowAllInfos()
        {
            if (QuestUIManager.Instance == null || QuestManager.Instance == null)
            {
                Debug.LogError("QuestUIManager or QuestManager instance is not available.");
                return;
            }
            QuestUIManager.Instance.ShowSelectedQuest(questID);

            // Accept Button
            if (QuestManager.Instance.RequestAvailableQuest(questID))
            {
                SetButtonState(QuestUIManager.Instance.acceptButton, true, questID);
            }
            else
            {
                SetButtonState(QuestUIManager.Instance.acceptButton, false);
            }

            // GIVE UP BUTTON
            if (QuestManager.Instance.RequestAcceptedQuest(questID))
            {
                SetButtonState(QuestUIManager.Instance.giveUpButton, true, questID);
            }
            else
            {
                SetButtonState(QuestUIManager.Instance.giveUpButton, false);
            }

            // COMPLETE BUTTON
            if (QuestManager.Instance.RequestCompleteQuest(questID))
            {
                SetButtonState(QuestUIManager.Instance.completeButton, true, questID);
            }
            else
            {
                SetButtonState(QuestUIManager.Instance.completeButton, false);
            }
        }

        // Helper method to set button state
        private void SetButtonState(GameObject button, bool isActive, string questID = null)
        {
            if (button == null) return;

            button.SetActive(isActive);

            if (isActive && questID != null)
            {
                QButtonScript buttonScript = button.GetComponent<QButtonScript>();
                if (buttonScript != null)
                {
                    buttonScript.questID = questID;
                }
            }
        }

        public void AcceptQuest()
        {
            if (QuestManager.Instance == null || QuestUIManager.Instance == null)
            {
                Debug.LogError("QuestManager or QuestUIManager instance is not available.");
                return;
            }

            QuestManager.Instance.AcceptQuest(QuestManager.Instance.GetQuestByID(questID));
            QuestUIManager.Instance.HideQuestPanel();
            UpdateNPCQuestMarkers(); // Update all NPC quest markers

            //// UPDATE ALL NPCS / QUEST GIVER
            //QuestObject[] currentQuestGuys = Object.FindObjectsByType<QuestObject>(FindObjectsSortMode.None);
            //foreach (QuestObject obj in currentQuestGuys)
            //{ 
            //    //setquestMarker
            //    obj.SetQuestMaker();
            //}
        }

        public void GiveUPQuest()
        {
            if (QuestManager.Instance == null || QuestUIManager.Instance == null)
            {
                Debug.LogError("QuestManager or QuestUIManager instance is not available.");
                return;
            }
            QuestManager.Instance.GiveUpQuest(QuestManager.Instance.GetQuestByID(questID));
            QuestUIManager.Instance.HideQuestPanel();
            UpdateNPCQuestMarkers();

            //// UPDATE ALL NPCS / QUEST GIVER
            //QuestObject[] currentQuestGuys = Object.FindObjectsByType<QuestObject>(FindObjectsSortMode.None);
            //foreach (QuestObject obj in currentQuestGuys)
            //{
            //    //setquestMarker
            //    obj.SetQuestMaker();
            //}
        }

        public void CompleteQuest()
        {
            if (QuestManager.Instance == null || QuestUIManager.Instance == null)
            {
                Debug.LogError("QuestManager or QuestUIManager instance is not available.");
                return;
            }

            QuestManager.Instance.CompleteQuest(QuestManager.Instance.GetQuestByID(questID.ToString()));
            QuestUIManager.Instance.HideQuestPanel();
            UpdateNPCQuestMarkers(); // Update all NPC quest markers

            //// UPDATE ALL NPCS / QUEST GIVER
            //QuestObject[] currentQuestGuys = Object.FindObjectsByType<QuestObject>(FindObjectsSortMode.None);
            //foreach (QuestObject obj in currentQuestGuys)
            //{
            //    //setquestMarker
            //    obj.SetQuestMaker();
            //}
        }

        public void ClosePanel()
        {
            if (QuestUIManager.Instance == null)
            {
                Debug.LogError("QuestUIManager instance is not available.");
                return;
            }

            QuestUIManager.Instance.HideQuestPanel();

            if (acceptButton != null) acceptButton.SetActive(false);
            if (giveUpButton != null) giveUpButton.SetActive(false);
            if (completeButton != null) completeButton.SetActive(false);
        }

        // Helper method to update NPC quest markers
        private void UpdateNPCQuestMarkers()
        {
            QuestObject[] currentQuestGuys = Object.FindObjectsByType<QuestObject>(FindObjectsSortMode.None);
            foreach (QuestObject obj in currentQuestGuys)
            {
                obj.SetQuestMaker(); // Update the NPC quest marker
            }
        }
    }
}
