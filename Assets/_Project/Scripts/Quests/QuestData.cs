using UnityEngine;
using System.Collections.Generic;
using System;


namespace Platformer
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Create New Quest")]
    public class QuestData : ScriptableObject
    {
        // Basic quest information
        public string title; // Quest title
        public string id = Guid.NewGuid().ToString(); // Unique identifier
        public string description; // Quest description
        public string hint; // Hint for the quest
        public string congratulation; // Message upon completion
        public string summary; // Summary of quest goals

        // Quest objective details
        public string questObjective; // Objective (e.g., "Collect apples")
        public int questObjectiveRequirement; // Required count to complete the objective
        public int questObjectiveCount; // Current progress
        public QuestProgress progress = QuestProgress.NOT_AVAILABLE;

        // Rewards
        
        public int coinReward; // Coin reward
        public int itemRewardID; // Reward item ID (if applicable)

        // Chain quests
        public QuestData nextQuest; // Link to the next quest, if it's a chain quest

        // Multi-step objectives (optional)
        public List<string> linkedNPCIDs; // NPCs involved in the quest
        public int currentStage; // Current quest stage
        public List<string> stageDescriptions; // Descriptions for each stage

        // Helper property to check if quest is complete
        public bool IsComplete => questObjectiveCount >= questObjectiveRequirement;

        // Advance quest stage (used for multi-step quests)
        public void AdvanceStage()
        {
            currentStage++;
            if (currentStage >= stageDescriptions.Count)
            {
                progress = QuestProgress.COMPLETE; // Mark quest as complete
                Debug.Log($"Quest '{title}' has been completed!");
            }
            else
            {
                Debug.Log($"Quest '{title}' advanced to stage: {stageDescriptions[currentStage]}");
            }
        }
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                Debug.Log($"Generated new Quest ID: {id} for quest '{title}'");
            }
        }

    }

    // Enum for quest progress states
    public enum QuestProgress
    {
        NOT_AVAILABLE,
        AVAILABLE,
        ACCEPTED,
        COMPLETE,
        DONE
    }
}