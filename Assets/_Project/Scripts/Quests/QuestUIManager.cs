using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Platformer
{
    public class QuestUIManager : MonoBehaviour
    {
        public static QuestUIManager Instance;

        // BOOLS
        public bool questAvailable = false;
        public bool questRunning = false;
        public bool questPanelActive = false;
        private bool questLogPannelActive = false;

        // PANELS
        public GameObject questPanel;
        public GameObject questLogPanel;

        // QUESTLIST
        public List<QuestData> availableQuests = new List<QuestData>();
        public List<QuestData> activeQuests = new List<QuestData>();

        //BUTTONS
        public GameObject qButton;
        public GameObject qLogButton;
        private List<GameObject> qButtons = new List<GameObject>();

        public GameObject acceptButton;
        public GameObject giveUpButton;
        public GameObject completeButton;

        // SPACER
        public Transform qButtonSpacer1; //qButton available
        public Transform qButtonSpacer2; //running qButton
        public Transform qLogButtonSpacer; //qButton active

        // QUEST INFOS
        public TextMeshProUGUI questTitle;
        public TextMeshProUGUI questDescription;
        public TextMeshProUGUI questSummary;

        // QUEST LOG INFOS
        public TextMeshProUGUI questLogTitle;
        public TextMeshProUGUI questLogDescription;
        public TextMeshProUGUI questLogSummary;

        //public QButtonScript acceptButtonScript;
        //public QButtonScript giveUpButtonScript;
        //public QButtonScript completeButtonScript;

        void Awake()
        {
            if (Instance == null || Instance == this)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            HideQuestPanel();
        }

        void Start()
        {
            //if (acceptButton != null)
            //{
            //    //acceptButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("AcceptButton").gameObject;
            //    acceptButtonScript = acceptButton.GetComponent<QButtonScript>();
            //    acceptButton.SetActive(false);
            //}
            //else
            //{
            //    Debug.LogWarning("AcceptButton is not assigned in the Inspector.");
            //}

            //if (giveUpButton != null)
            //{
            //    //giveUpButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("GiveUpButton").gameObject;
            //    giveUpButtonScript = giveUpButton.GetComponent<QButtonScript>();
            //    giveUpButton.SetActive(false);
            //}

            //if (completeButton != null)
            //{
            //    //completeButton = GameObject.Find("QuestCanvas").transform.Find("QuestPanel").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("CompleteButton").gameObject;
            //    completeButtonScript = completeButton.GetComponent<QButtonScript>();
            //    completeButton.SetActive(false);
            //}
            //else
            //{ 
            //    Debug.LogWarning("CompleteButton is not assigned in the Inspector.");
            //}
            if (acceptButton != null) acceptButton.SetActive(false);
            if (giveUpButton != null) giveUpButton.SetActive(false);
            if (completeButton != null) completeButton.SetActive(false);
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                questLogPannelActive = !questPanelActive;
                // Show the quest log panel
                ShowQuestLogPanel();
            }
        }

        // CALLED FROM QuestObject
        public void ShowAvailableQuests(QuestObject questObject)
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogError("QuestManager instance is not available.");
                return;
            }

            availableQuests.Clear(); // Clear the list to avoid duplicates

            // Retrieve and process available quests
            foreach (int questID in questObject.availableQuestIDs)
            {
                QuestData quest = QuestManager.Instance.GetQuestByID(questID.ToString());
                if (quest != null && quest.progress == QuestProgress.AVAILABLE)
                {
                    availableQuests.Add(quest);
                    Debug.Log($"Quest '{quest.title}' is available!");
                }
            }

            // Check and display the quest panel
            if (availableQuests.Count > 0)
            {
                ShowQuestPanel();
            }
            else
            {
                Debug.Log("No quests available to display.");
            }
        }

        // SHOW PANEL
        public void ShowQuestPanel()
        {
            questPanelActive = true;
            questPanel.SetActive(questPanelActive);

            ClearButtons();

            //Create buttons for available quests
            foreach (QuestData curQuest in availableQuests)
            {
                CreateQuestButton(curQuest, qButtonSpacer1);
            }

            // Create buttons for active quests
            foreach (QuestData curQuest in activeQuests)
            {
                CreateQuestButton(curQuest, qButtonSpacer2);
            }
        }

        public void ShowQuestLogPanel()
        {
            questLogPanel.SetActive(questLogPannelActive);
            // Populate the quest log when active
            if (questLogPannelActive)
            {
                ClearButtons();

                // Populate the quest log
                foreach (QuestData curQuest in QuestManager.Instance.CurrentQuestList)
                {
                    CreateQuestButton(curQuest, qLogButtonSpacer);
                    //GameObject questButton = Instantiate(qLogButton);
                    //QLogButtonScript qbutton = questButton.GetComponent<QLogButtonScript>();

                    //qbutton.questID = curQuest.id;
                    //qbutton.questTitle.text = curQuest.title; 

                    //questButton.transform.SetParent(qLogButtonSpacer, false);
                    //qButtons.Add(questButton);
                }
            }
            else
            {
                HideQuestLogPanel();
            }
        }

        public void ShowQuestLog(QuestData activeQuest)
        {
            questLogTitle.text = activeQuest.title;
            if (activeQuest.progress == QuestProgress.ACCEPTED)
            {
                questLogDescription.text = activeQuest.hint;
                questLogSummary.text = activeQuest.questObjective + " : " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
            }
            else if (activeQuest.progress == QuestProgress.COMPLETE)
            {
                questLogDescription.text = activeQuest.congratulation;
                questLogSummary.text = activeQuest.questObjective + " : " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
            }
        }

        // BUTTON ACTIONS
        //public void OnAcceptButtonClick()
        //{

        //    QuestManager.Instance.AcceptQuest(assignedQuest); // Replace with appropriate quest selection logic
        //    Debug.Log("Accept Button Clicked!");
        //    acceptButton.SetActive(false); // Disable after acceptance
        //}


        // HIDE QUEST PANEL
        public void HideQuestPanel()
        {
            questPanelActive = false;
            questPanel.SetActive(false);
            //questAvailable = false;
            //questRunning = false;

            ////CLEAR TEXT
            //questTitle.text = "";
            //questDescription.text = "";
            //questSummary.text = "";

            //// CLEAR LISTS
            //availableQuests.Clear();
            //activeQuests.Clear();

            // CLEAR BUTTON LIST
            ClearButtons();
        }

        // HIDE QUEST LOG PANEL
        public void HideQuestLogPanel()
        {
            questLogPannelActive = false;

            questLogTitle.text = "";
            questLogDescription.text = "";
            questLogSummary.text = "";

            ClearButtons();
            questLogPanel.SetActive(false);
        }

        // CLEAR BUTTONS
        void ClearButtons()
        {
            foreach (GameObject button in qButtons)
            {
                Destroy(button);
            }
            qButtons.Clear();
        }

        void CreateQuestButton(QuestData quest, Transform parentSpacer)
        {
            if (qButton == null)
            {
                Debug.LogError("qButton prefab is not assigned in the Inspector.");
                return;
            }
            GameObject questButton = Instantiate(qButton);
            QButtonScript qBScript = questButton.GetComponent<QButtonScript>();

            qBScript.questID = quest.id;
            qBScript.questTitle.text = quest.title;

            questButton.transform.SetParent(parentSpacer.transform, false);
            qButtons.Add(questButton);
        }

        //RefreshQuestLog
        //public void RefreshQuestLog()
        //{
        //    ClearButtons();

        //    foreach (QuestData quest in QuestManager.Instance.currentQuestList)
        //    {
        //        GameObject questButton = Instantiate(qLogButton);
        //        QLogButtonScript qBScript = questButton.GetComponent<QLogButtonScript>();

        //        qBScript.questID = quest.id;
        //        qBScript.questTitle.text = quest.title;

        //        questButton.transform.SetParent(qLogButtonSpacer, false);
        //        qButtons.Add(questButton);
        //    }
        //}

        public void ShowQuestInProgress(QuestData quest)
        {
            // Log that this quest is already in progress
            Debug.Log($"Quest '{quest.title}' is in progress.");

            // Update the quest panel with the quest's information
            questTitle.text = quest.title;
            questDescription.text = quest.description;
            questSummary.text = quest.questObjective + " : " + quest.questObjectiveCount + " / " + quest.questObjectiveRequirement;

            // Hide the accept button, since the quest is already active
            acceptButton.SetActive(false);

            // Optionally show buttons for completing or giving up the quest, based on its state
            if (quest.progress == QuestProgress.ACCEPTED)
            {
                giveUpButton.SetActive(true);
                completeButton.SetActive(false); // Cannot complete yet
            }
            else if (quest.progress == QuestProgress.COMPLETE)
            {
                giveUpButton.SetActive(false); // Cannot give up a completed quest
                completeButton.SetActive(true);
            }

            // Ensure the panel is active to display the details
            questPanelActive = true;
            questPanel.SetActive(true);
        }
        public void ShowQuestAvailable(QuestData quest)
        {
            // Log that this quest is available
            Debug.Log($"Quest '{quest.title}' is available.");

            // Update the quest panel with the quest's information
            questTitle.text = quest.title;
            questDescription.text = quest.description;
            questSummary.text = quest.questObjective + " : " + quest.questObjectiveRequirement;

            // Show the Accept button for the available quest
            acceptButton.SetActive(true);

            // Hide other buttons since the quest is not active yet
            giveUpButton.SetActive(false);
            completeButton.SetActive(false);

            // Ensure the panel is active to display the details
            questPanelActive = true;
            questPanel.SetActive(true);
        }

        // SHOW QUEST ON BUTTON PRESS IN QUEST PANEL

        public void ShowSelectedQuest(string questID)
        {
            foreach (QuestData quest in availableQuests)
            {

                if (quest.id == questID)
                {
                    questTitle.text = quest.title;
                    questDescription.text = quest.description;
                    questSummary.text = quest.questObjective + " : " + quest.questObjectiveCount + " / " + quest.questObjectiveRequirement;
                    return; // Prevent overwriting
                }
            }


            foreach (QuestData quest in activeQuests)
            {
                if (quest.id == questID)
                {
                    questTitle.text = quest.title;
                    if (quest.progress == QuestProgress.ACCEPTED)
                    {
                        questDescription.text = quest.hint;
                        questSummary.text = quest.questObjective + " : " + quest.questObjectiveCount + " / " + quest.questObjectiveRequirement;
                    }
                    else if (quest.progress == QuestProgress.COMPLETE)
                    {
                        questDescription.text = quest.congratulation;
                        questSummary.text = quest.questObjective + " : " + quest.questObjectiveCount + " / " + quest.questObjectiveRequirement;
                    }
                    return; // Prevent overwriting
                }
            }
        }
    }
}
