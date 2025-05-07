using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class DialogueUIManager : MonoBehaviour 
    {
        public static DialogueUIManager Instance;

        [Header("UI Elements")]
        public VisualElement dialoguePanel; // This is for the dialogue UI panel
        public Label npcNameText; //This will serve as te NPC name
        public Label currentSentence; // This will serve as the current sentence text
        public Button nextButton; // Button to proceed to the next sentence
        //public Button previousButton; // Button to go back to the previous sentence
        public Button closeButton; // Button to close the dialogue panel

        private DialogueSO currentDialogue; // Current dialogue being displayed
        private int currentSentenceIndex = 0; // Index of the current sentence being displayed

        [Header("Events")]
        [SerializeField] private BoolEventChannel dialogueEventChannel;

        [SerializeField] private CutsceneEventChannel cutsceneEventChannel;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Initialize UI Toolkit elements
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument != null)
            {
                var root = uiDocument.rootVisualElement;

                dialoguePanel = root.Q<VisualElement>("DialoguePanel");
                npcNameText = root.Q<Label>("NPCNameText");
                currentSentence = root.Q<Label>("CurrentSentence");
                closeButton = root.Q<Button>("closeButton");
                nextButton = root.Q<Button>("NextButton");

                // button click events
                nextButton.clicked += NextSsentence;
                //closeButton.clicked += EndDialogue;

                // Hiding the panel at the start
                dialoguePanel.style.display = DisplayStyle.None;
            }
            else
            {
                Debug.LogError("UIDocument is not attached to the GameObject.");
            }
        }

        public void StartDialogue(DialogueSO dialogue)
        {
            currentDialogue = dialogue;
            currentSentenceIndex = 0;
            dialoguePanel.style.display = DisplayStyle.Flex;

            npcNameText.text = dialogue.name;

            DisplayCurrentSentence();

            Debug.Log("Invoking DialogueEventChannel: true");
            dialogueEventChannel?.Invoke(true);
        }

        public void DisplayCurrentSentence()
        {
            //Debug.Log($"Displaying sentence {currentSentenceIndex + 1} of {currentDialogue.Sentence.Count}");
            if (currentDialogue == null || currentSentenceIndex >= currentDialogue.sentencesData.Count)
            {
                EndDialogue();
                return;
            }
            //Debug.Log("Starting");
            currentSentence.text = currentDialogue.sentencesData[currentSentenceIndex].sentence;

            if(currentDialogue.sentencesData[currentSentenceIndex].cutscene != null)
            {
                Debug.Log($"Triggering cutscene: {currentDialogue.sentencesData[currentSentenceIndex].cutscene.id}");
                cutsceneEventChannel?.Invoke(currentDialogue.sentencesData[currentSentenceIndex].cutscene);
            }
            
        }

        public void PauseDialogue()
        {
            dialoguePanel.SetEnabled(false);
        }

        public void ResumeDialogue()
        {
            dialoguePanel.SetEnabled(true);
        }


        public void NextSsentence()
        {
            if (currentSentenceIndex < currentDialogue.sentencesData.Count - 1)
            {
                currentSentenceIndex++;
                DisplayCurrentSentence();

                // If this is the last sentence, update the button text to "Close"
                if (currentSentenceIndex == currentDialogue.sentencesData.Count - 1)
                {
                    nextButton.text = "Close";
                }
            }
            else
            {
                EndDialogue();

                nextButton.text = "Next"; // Reset the button text for the next dialogue
            }
        }

        //public void PreviousSentence()
        //{
        //    if (currentSentenceIndex > 0)
        //    {
        //        currentSentenceIndex--;
        //        DisplayCurrentSentence();
        //    }
        //    else
        //    {
        //        Debug.Log("Already at the first sentence.");
        //    }
            
        //}

        public void EndDialogue()
        {
            dialoguePanel.style.display = DisplayStyle.None;
            currentDialogue = null;
            Debug.Log("Invoking DialogueEventChannel: false");
            dialogueEventChannel?.Invoke(false);
        }


    }
}
