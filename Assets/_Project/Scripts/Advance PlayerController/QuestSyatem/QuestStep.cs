using UnityEngine;

namespace Platformer
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;

        protected void FinishQuestStep()
        {
            if (!isFinished)
            {
                isFinished = true;

                //TODO - Advance the quest forward now that we've finished this step

                Destroy(this.gameObject);
            }
        }
    }
}
