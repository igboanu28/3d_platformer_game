using UnityEngine;

namespace Platformer
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/Quest/Advance QuestInfoSO", order = 1)]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string id { get; private set; }

        [Header("General")]
        public string displayName;

        [Header("Requirements")]
        public int levelRequirement;

        // this are like the requirement
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Rewards")]
        public int coinReward;
        public int experienceReward;

        // ensure the id is always the name of the Scriptable Object asset
        private void OnValidate()
        {
            #if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
