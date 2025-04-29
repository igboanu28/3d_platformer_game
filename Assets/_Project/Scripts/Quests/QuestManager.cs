using UnityEngine;
using System.Collections.Generic;

namespace Platformer
{
    public class QuestManager : MonoBehaviour
    {
        // Singleton Instance
        public static QuestManager Instance { get; private set; }
        public List<QuestData> CurrentQuestList => new List<QuestData>(questDictionary.Values);

        // Dictionary for faster quest retrieval
        private Dictionary<string, QuestData> questDictionary = new Dictionary<string, QuestData>(); // Added for optimization

        public QuestEventChannel questAcceptedChannel; // Event channel for quest acceptance
        public QuestEventChannel questCompletedChannel; // Event channel for quest completion

        //private vars for the QuestObject
        void Awake()
        {
            if (Instance == null || Instance == this)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("Duplicate QuestManager instance detected. Destroying this instance."); // Improved error handling
                Destroy(gameObject);
                return;
            }

            // Debugging currentQuestList contents
            foreach (var quest in questDictionary.Values)
            {
                Debug.Log($"Quest Initialized: ID = {quest.id}, Title = {quest.title}, Progress = {quest.progress}");
            }
        }

        void Start()
        {
            // Pass an empty list to InitializeQuests to resolve the error
            InitializeQuests(new List<QuestData>());
        }

        public void InitializeQuests(List<QuestData> quests)
        {
            foreach (var quest in quests)
            {
                if (!questDictionary.ContainsKey(quest.id))
                {
                    questDictionary[quest.id] = quest;
                    Debug.Log($"Quest '{quest.title}' initialized with ID: {quest.id}");
                }
            }
        }

        // Optimized GetQuestByID using dictionary
        public QuestData GetQuestByID(string questID, bool logWarning = true)
        {
            if (questDictionary.TryGetValue(questID, out QuestData quest))
            {
                return quest;
            }

            if (logWarning)
            {
                Debug.LogWarning($"Quest with ID {questID} not found. Current dictionary contains {questDictionary.Count} entries.");
            }
            return null; // Return null gracefully
        }

        // Accept Quest
        public void AcceptQuest(QuestData quest)
        {
            if (questDictionary.ContainsKey(quest.id))
            {
                Debug.LogWarning($"Quest '{quest.title}' is already accepted.");
                return; // Prevent further processing
            }

            // Create a runtime copy of the quest
            QuestData runtimeQuest = Instantiate(quest);
            runtimeQuest.progress = QuestProgress.ACCEPTED;

            questDictionary[runtimeQuest.id] = runtimeQuest;
            Debug.Log($"Quest '{runtimeQuest.title}' accepted!");

            if (quest.progress != QuestProgress.AVAILABLE)
            {
                Debug.LogWarning($"Quest '{quest.title}' is not in an available state. Current state: {quest.progress}");
                return; // this prevent accepting quests not available
            }

            questDictionary[quest.id] = quest; // Update dictionary
            quest.progress = QuestProgress.ACCEPTED; // this ensures that the progress is updated
            if (questAcceptedChannel != null)
            {
                questAcceptedChannel.Invoke(quest); // Broadcast quest accepted event
            }
            else
            {
                Debug.LogWarning("QuestAcceptedChannel is null. Event not invoked.");
            }
                Debug.Log($"Quest '{quest.title}' accepted!");
        }

        // Give up a quest
        public void GiveUpQuest(QuestData quest)
        {
            if (questDictionary.TryGetValue(quest.id, out QuestData existingQuest) && existingQuest.progress == QuestProgress.ACCEPTED)
            {
                existingQuest.progress = QuestProgress.AVAILABLE;
                existingQuest.questObjectiveCount = 0;
                questDictionary.Remove(quest.id); // Update dictionary
                Debug.Log($"Quest '{quest.title}' was given up.");
            }
        }

        // Complete a quest
        public void CompleteQuest(QuestData quest)
        {
            if (questDictionary.TryGetValue(quest.id, out QuestData existingQuest) && existingQuest.IsComplete)
            {
                existingQuest.progress = QuestProgress.DONE;
                if (questCompletedChannel != null)
                {
                    questCompletedChannel.Invoke(existingQuest); // Broadcast quest completed event
                }
                else
                {
                    Debug.LogWarning("QuestCompletedChannel is null. Event not invoked.");
                }
                    questDictionary.ContainsKey(quest.id);

                //// Reward the player
                //if (PlayerController.Instance != null)
                //{
                //    PlayerController.Instance.AddCoins(existingQuest.coinReward);
                //}
                //else
                //{
                //    Debug.LogWarning("PlayerController instance not found. Rewards cannot be delivered.");
                //}
                Debug.Log($"Quest '{existingQuest.title}' completed!");
                // Check for chain quests
                CheckChainQuest(existingQuest);
            }
        }

       

        // Check for chain quests (multi-step quests)
        private void CheckChainQuest(QuestData quest)
        {
            if (quest.nextQuest != null && quest.progress == QuestProgress.DONE)
            {
                quest.nextQuest.progress = QuestProgress.AVAILABLE;
                Debug.Log($"Next quest '{quest.nextQuest.title}' is now available! (Chain from: {quest.title})");
            }
            else
            {
                Debug.LogWarning($"Chain quest for '{quest.title}' is not properly configured.");
            }
        }

        //ADD ITEMS TO THE QUEST LIST
        public void AddQuestItem(string questObjective, int itemAmount)
        {
            foreach (var quest in CurrentQuestList)
            {
                if (quest.questObjective == questObjective && quest.progress == QuestProgress.ACCEPTED)
                {
                    quest.questObjectiveCount += itemAmount;
                    if (quest.questObjectiveCount >= quest.questObjectiveRequirement)
                    {
                        quest.progress = QuestProgress.COMPLETE;
                        Debug.Log($"Quest '{quest.title}' marked as complete!");
                        CompleteQuest(quest);
                    }
                }
            }
        }

        // BOOL
        // Check if a quest is available
        public bool RequestAvailableQuest(string questID)
        {
            return questDictionary.TryGetValue(questID, out QuestData quest) && quest.progress == QuestProgress.AVAILABLE;
        }

        // Check if a quest is currently accepted

        public bool RequestAcceptedQuest(string questID)
        {
            return questDictionary.TryGetValue(questID, out QuestData quest) && quest.progress == QuestProgress.ACCEPTED;
        }

        // Check if a quest is complete

        public bool RequestCompleteQuest(string questID)
        {
            return questDictionary.TryGetValue(questID, out QuestData quest) && quest.progress == QuestProgress.COMPLETE;
        }

        //Check available quests
        public bool CheckAvailableQuests(QuestObject NPCQuestObject)
        {
            foreach (int questID in NPCQuestObject.availableQuestIDs)
            {
                if (questDictionary.TryGetValue(questID.ToString(), out QuestData quest) && quest.progress == QuestProgress.AVAILABLE)
                {
                    return true;
                }
            }
            return false;
        }

        // Check accepted quests
        public bool CheckAcceptedQuests(QuestObject NPCQuestObject)
        {
            foreach (int questID in NPCQuestObject.receivableQuestIDs)
            {
                if (questDictionary.TryGetValue(questID.ToString(), out QuestData quest) && quest.progress == QuestProgress.ACCEPTED)
                {
                    return true;
                }
            }
            return false;
        }

        // Check completed quests
        public bool CheckCompletedQuests(QuestObject NPCQuestObject)
        {
            foreach (int questID in NPCQuestObject.receivableQuestIDs)
            {
                if (questDictionary.TryGetValue(questID.ToString(), out QuestData quest) && quest.progress == QuestProgress.COMPLETE)
                {
                    return true;
                }
            }
            return false;
        }

        //// SHOW QUEST LOG
        //public void ShowQuestLog(string questID)
        //{
        //    if (questDictionary.TryGetValue(questID, out QuestData quest))
        //    {
        //        QuestUIManager.Instance.ShowQuestLog(quest);
        //    }
        //}



        ////QuestRequest
        //public void QuestRequest(QuestObject NPCQuestObject)
        //{
        //    // Debugging availableQuestIDs
        //    foreach (int questID in NPCQuestObject.availableQuestIDs)
        //    {
        //        if (questDictionary.TryGetValue(questID.ToString(), out QuestData quest) && quest.progress == QuestProgress.AVAILABLE)
        //        {
        //            QuestUIManager.Instance.ShowQuestAvailable(quest);
        //        }
        //    }

        //    foreach (int questID in NPCQuestObject.receivableQuestIDs)
        //    {
        //        if (questDictionary.TryGetValue(questID.ToString(), out QuestData quest) && (quest.progress == QuestProgress.ACCEPTED || quest.progress == QuestProgress.COMPLETE))
        //        {
        //            QuestUIManager.Instance.ShowQuestInProgress(quest);
        //        }
        //    }
        //}
    }
}