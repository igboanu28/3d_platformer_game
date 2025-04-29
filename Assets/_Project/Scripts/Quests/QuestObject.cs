using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Platformer
{
    public class QuestObject : MonoBehaviour
    {
        private bool inTrigger = false;

        public List<int> availableQuestIDs = new List<int>();
        public List<int> receivableQuestIDs = new List<int>();

        public GameObject questMarker;
        public Image questMarkerImage;

        public Sprite questAvailableSprite;
        public Sprite questReceivableSprite;

        void Start()
        {
            if (questMarker == null || questMarkerImage == null)
            {
                Debug.LogError("Quest marker or quest marker image is not assigned in the Inspector.");
                return;
            }
            SetQuestMaker();
        }

        public void SetQuestMaker()
        {
            Debug.Log("Setting quest marker for NPC...");
            Debug.Log($"Available Quest IDs: {string.Join(", ", availableQuestIDs)}");
            Debug.Log($"Receivable Quest IDs: {string.Join(", ", receivableQuestIDs)}");   

            bool markerSet = false;

            // Check for complete quests
            if (QuestManager.Instance.CheckCompletedQuests(this))
            {
                UpdateQuestMarker(questReceivableSprite, Color.red);
                Debug.Log("Completed Quests: True");
                markerSet = true;
            }
            // Check for available quests
            else if (QuestManager.Instance.CheckAvailableQuests(this))
            {
                UpdateQuestMarker(questAvailableSprite, Color.yellow);
                Debug.Log("Available Quests: True");
                markerSet = true;
            }
            // check for accepted quests
            else if (QuestManager.Instance.CheckAcceptedQuests(this))
            {
                UpdateQuestMarker(questReceivableSprite, Color.blue);
                Debug.Log("Accepted Quests: True");
                markerSet = true;
            }

            questMarker.SetActive(markerSet);
            if (!markerSet)
            {
                Debug.Log("No quests to display for NPC.");

            }
        }

        void UpdateQuestMarker(Sprite sprite, Color color)
        {
            if (sprite == null)
            { 
                Debug.LogError("Quest marker sprite is not assigned.");
                return;
            } 

            questMarker.SetActive(true);
            questMarkerImage.sprite = sprite;
            questMarkerImage.color = color;
            Debug.Log($"Quest marker updated: Sprite = {sprite.name}, Color = {color}");
        }

        void Update()
        {
            if (inTrigger && Input.GetKeyDown(KeyCode.T))
            {
                if (QuestUIManager.Instance == null)
                { 
                    Debug.LogError("QuestUIManager instance is null.");
                    return;
                }
                if (!QuestUIManager.Instance.questPanelActive)
                {
                    //quest ui manager
                    QuestUIManager.Instance.ShowAvailableQuests(this);
                    Debug.Log($"Player interacted with NPC: {gameObject.name}");

                    //Debug.Log("Player interacted with QuestObject.");
                    //QuestManager.questManager.QuestRequest(this);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                inTrigger = true;
                //SetQuestMaker();
                Debug.Log($"Player entered NPC trigger zone: {gameObject.name}");

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inTrigger = false;
                Debug.Log($"Player exited NPC trigger zone: {gameObject.name}");
            }
        }
    }
}