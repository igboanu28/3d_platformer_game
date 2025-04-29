using System.Collections.Generic;

namespace DialogueSystem
{
    public class DialogueHistory 
    {
        private HashSet<string> completedDialogues = new HashSet<string>();

        public bool isDialogueCompleted(DialogueSO dialogue)
        {
            return completedDialogues.Contains(dialogue.id);
        }

        public void MarkDialogueAsCompleted(DialogueSO dialogue)
        {
            completedDialogues.Add(dialogue.id);
        }
    }
}
