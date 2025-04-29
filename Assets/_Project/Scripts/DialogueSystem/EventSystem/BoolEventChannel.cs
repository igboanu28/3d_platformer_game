using System.Diagnostics.Tracing;
using UnityEngine;
using Platformer;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Events/BoolEventChannel")]

    public class BoolEventChannel : EventChannel<bool>
    {

    }
}
