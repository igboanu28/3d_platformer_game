using UnityEngine;
using System;
using System.Collections.Generic;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueSO")]
    public class DialogueSO : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString(); // Unique identifier
        public string npcName; // Name of the NPC 
        //public List<string> Sentence;

        //// this define the trigger word or phrase
        //public List<CutsceneSO> cutscenes;

        public List<SentenceData> sentencesData;
    }

    [Serializable]
    public class  SentenceData
    {
        [SerializeField]public string sentence;
        [SerializeField] public CutsceneSO cutscene;
    }
}
