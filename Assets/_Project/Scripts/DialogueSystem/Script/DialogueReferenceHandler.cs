using UnityEngine;
using DialogueSystem;
using Platformer;

namespace DialogueSystem
{
    public class DialogueReferenceHandler : MonoBehaviour
    {
        //public static DialogueReferenceHandler Instance;

        [Header("Dialogue Reference")]
        public DialogueSO dialogue; // This is a reference to the DialogueSO scriptable object
        public DialogueListSO dialogueList; // This is a reference to the DialogueListSO scriptable object

        public void Start()
        {
            if (dialogue == null)
            {
                Debug.LogError("Dialogue is not assigned.");
                return;
            }

            Debug.Log($"Starting Dialogue with ID: {dialogue.id} (NAME: {dialogue.npcName})");
            foreach (var sentence in dialogue.Sentence)
            {
                Debug.Log($"Dialogue Sentence: {sentence}");
            }
        }
        public void StartDialogue()
        {
            if (DialogueUIManager.Instance != null)
            {
                
                DialogueUIManager.Instance.StartDialogue(dialogue);
            }
        }

        
    }
}
