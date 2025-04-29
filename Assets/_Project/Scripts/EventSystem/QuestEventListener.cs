using UnityEngine;

namespace Platformer
{
    public class QuestEventListener : EventListener<QuestData>
    {
        [SerializeField] private QuestEventChannel questEventChannel; // Specific to QuestEventChannel

        protected new void Awake()
        {
            if (questEventChannel != null)
            {
                questEventChannel.Register(this);
            }
            else
            {
                Debug.LogError($"{nameof(QuestEventListener)}: QuestEventChannel is not assigned on {gameObject.name}");
            }
        }

        protected new void OnDestroy()
        {
            if (questEventChannel != null)
            {
                questEventChannel.Deregister(this);
            }
        }

        public new void Raise(QuestData quest)
        {
            Debug.Log($"QuestEventListener: Event raised for quest '{quest.title}'");
            base.Raise(quest); // Invoke the UnityEvent
        }
    }
}