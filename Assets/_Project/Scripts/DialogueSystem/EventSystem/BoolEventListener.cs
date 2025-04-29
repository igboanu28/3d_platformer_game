using UnityEngine;
using Platformer;

namespace DialogueSystem
{
    public class BoolEventListener : EventListener<bool>
    {
        public override void Raise(bool value)
        {
            Debug.Log($"BoolEventListener: Raising event with value: {value}");
            base.Raise(value);

        }
    }
}
