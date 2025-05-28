using UnityEngine;
using UnityEngine.Playables;
using System;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "CutScene", menuName = "Scriptable Objects/CutsceneSO")]
    public class CutsceneSO : ScriptableObject 
    {
        public string id = Guid.NewGuid().ToString(); // Unique identifier
        public PlayableAsset timeline; // Reference to the timeline asset
    }
}
