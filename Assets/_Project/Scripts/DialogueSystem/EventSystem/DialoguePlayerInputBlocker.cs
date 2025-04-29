//using Platformer;
//using UnityEngine.Events;
//using UnityEngine;

//namespace DialogueSystem
//{
//    public class DialoguePlayerInputBlocker : DialogEventListener
//    {
//        [SerializeField] private InputReader inputReader;

//        private void OnEnable()
//        {
//            // Add listener to the UnityEvent via code
//            // This assumes unityEvent is exposed or can be serialized in EventListener<T>
//            // You can alternatively wire it from the inspector
//            unityEvent.AddListener(HandleDialogueInput);
//        }

//        private void OnDisable()
//        {
//            unityEvent.RemoveListener(HandleDialogueInput);
//        }

//        private void HandleDialogueInput(bool isDialogueOpen)
//        {
//            Debug.Log($"Dialogue state changed: {isDialogueOpen}");

//            if (inputReader != null)
//            {
//                if (isDialogueOpen)
//                {
//                    Debug.Log("Disabling player actions.");
//                    inputReader.DisablePlayerActions();
//                }
//                else
//                {
//                    Debug.Log("Enabling player actions.");
//                    inputReader.EnablePlayerActions();
//                }
//            }
//            else
//            {
//                Debug.LogWarning("InputReader not assigned.");
//            }
//        }
//    }
//}