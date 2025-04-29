using UnityEngine;

namespace DialogueSystem
{
    public class  NPCInteractionHandler : MonoBehaviour
    {
        public DialogueReferenceHandler dialogueReferenceHandler; // Reference to the DialogueReferenceHandler script
        private bool inTrigger = false;

        void Update()
        {
            // Check if the player is in the trigger zone and the user pressed the "C" key
            if (inTrigger && Input.GetKeyDown(KeyCode.C))
            {
                if (dialogueReferenceHandler != null)
                { 
                    dialogueReferenceHandler.StartDialogue(); // Start the dialogue
                }
                else
                {
                    Debug.LogError("DialogueReferenceHandler is not assigned.");
                }
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inTrigger = true; // Player is in the trigger zone
                Debug.Log("Press 'C' to interact with the NPC.");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inTrigger = false; // Player is out of the trigger zone
                Debug.Log("You have exited the interaction zone.");
            }
        }
    }
}
