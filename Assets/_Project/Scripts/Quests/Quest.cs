//using UnityEngine;

//namespace Platformer
//{
//    [System.Serializable]
//    public class Quest 
//    {
//        public enum QuestProgress
//        {
//            NOT_AVAILABLE,
//            AVAILABLE,
//            ACCEPTED,
//            COMPLETE,
//            DONE
//        }

//        public string title; // name / title for the quest
//        public int id; // unique identifier for the quest
//        public QuestProgress progress; //state of the current quest (which i am using an enum for example)
//        public string description; //string for the description of the quest for the quest giver/receiver
//        public string hint; //string for the hint of the quest for the quest giver/receiver
//        public string congratulation; //string for the congratulation of the quest for the quest giver/receiver
//        public string summary; //sumaarizes the quest
//        public int nextQuest; //the next quest if there is any (chain quest)

//        public string questObjective; //name of the quest objective (aldo for remove)
//        public int questObjectiveCount; //how many of the quest objective is needed
//        public int questObjectiveRequirement; //required amount of the quest objective

//        public int experienceReward; //how much experience the quest gives
//        public int coinReward; //how much coin the quest gives
//        public int itemReward; //item reward for the quest
//    }
//}
