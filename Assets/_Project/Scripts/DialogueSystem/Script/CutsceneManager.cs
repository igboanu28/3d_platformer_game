using UnityEngine;
using UnityEngine.Playables;

namespace DialogueSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        public static CutsceneManager Instance;

        [Header("Cameras")]
        [SerializeField] private GameObject followCamera; // Main player-follow camera
        [SerializeField] private GameObject dollyCamera; // Cutscene camera

        [Header("Cutscene Event Channel")]
        [SerializeField] private CutsceneEventChannel cutsceneEventChannel;

        [Header("Player Director")]
        [SerializeField] private PlayableDirector director; // PlayableDirector for the cutscene

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            if (director != null)
            {
                director.stopped += OnCutsceneEnd;
            }
        }

        private void OnDisable()
        {
            if (director != null)
            {
                director.stopped -= OnCutsceneEnd;
            }
        }

        public void PlayCutscene(CutsceneSO cutscene)
        {
            if (cutscene == null || cutscene.timeline == null)
            {
                Debug.LogError("Cutscene or timeline is not assigned or Invalid.");
                return;
            }

            // Pause the dialogue UI
            if (DialogueUIManager.Instance != null)
            { 
                DialogueUIManager.Instance.PauseDialogue();
            }
           
            // Switch to dolly camera
            followCamera.SetActive(false);
            dollyCamera.SetActive(true);

            // Play the cutscene
            director.playableAsset = cutscene.timeline;
            director.Play();
        }

        private void OnCutsceneEnd(PlayableDirector _)
        { 
            // Switch back to follow camera
            followCamera.SetActive(true);
            dollyCamera.SetActive(false);

            // Resume dialogue if applicable
            if (DialogueUIManager.Instance != null)
            { 
                DialogueUIManager.Instance.ResumeDialogue();
            }
        }
    }
}
