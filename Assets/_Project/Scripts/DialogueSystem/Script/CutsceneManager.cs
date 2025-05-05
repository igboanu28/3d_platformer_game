using UnityEngine;
using UnityEngine.Playables;
using Platformer;

namespace DialogueSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        public static CutsceneManager Instance;

        [SerializeField] private CutsceneEventChannel cutsceneEventChannel;

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

        public void PlayCutscene(CutsceneSO cutscene)
        {
            if (cutscene == null || cutscene.timeline == null)
            {
                Debug.LogError("Cutscene or timeline is not assigned or Invalid.");
                return;
            }
            var playableDirector = Object.FindFirstObjectByType<PlayableDirector>();
            if (playableDirector != null)
            {
                playableDirector.playableAsset = cutscene.timeline;
                playableDirector.Play();
            }
            else
            {
                Debug.LogError("PlayableDirector not found in the scene.");
            }
        }
    }
}
