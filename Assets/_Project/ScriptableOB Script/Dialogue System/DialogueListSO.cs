using UnityEngine;
using System.Collections.Generic;
using System;
using DialogueSystem;


namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueListSO", menuName = "Scriptable Objects/Dialogue/DialogueListSO")]
    public class DialogueListSO : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString(); // Unique identifier

        public List<DialogueSO> dialogueList;
    }
}
