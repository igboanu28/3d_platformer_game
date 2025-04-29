using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] EventChannel<T> eventChannel;
        [SerializeField] UnityEvent<T> unityEvent = new();

        protected void Awake()
        {
            if (eventChannel != null)
            {
                eventChannel.Register(this);
            }
            else
            {
                Debug.LogError($"{nameof(EventListener<T>)}: EventChannel is not assigned on {gameObject.name}");
            }

        }

        protected void OnDestroy()
        {
            if (eventChannel != null)
            {
                eventChannel.Deregister(this);
            }

        }

        public virtual void Raise(T value)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(value);
            }
            else
            {
                Debug.LogWarning($"{nameof(EventListener<T>)}: UnityEvent is not assigned on {gameObject.name}");
            }

        }
    }

    public class EventListener : EventListener<Empty> { }
}