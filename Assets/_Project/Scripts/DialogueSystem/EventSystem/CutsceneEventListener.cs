using Platformer;
using System;
using UnityEngine;

namespace DialogueSystem
{
    public class CutsceneEventListener : EventListener<CutsceneSO>
    {
        [SerializeField] private CutsceneManager cutsceneManager;

        public override void Raise(CutsceneSO cutscene)
        {
            Debug.Log($"CutsceneEventListener: Raising cutscene event for {cutscene.id}");
            if (cutsceneManager != null)
            {
                cutsceneManager.PlayCutscene(cutscene);
            }
            else
            {
                Debug.LogError("CutsceneManager is not assigned in CutsceneEventListener.");
            }
        }
    }
}
