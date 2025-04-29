namespace UnityUtils.StateMachine
{
    public interface IStates
    {
        void Update() { }
        void FixedUpdate() { }
        void OnEnter() { }
        void OnExit() { }
    }
}