using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtils.StateMachine
{
    
    public class StateMachine
    {
        StateNode currentNode;
        readonly Dictionary<Type, StateNode> nodes = new();
        readonly HashSet<Transition> anyTransitions = new();

        public IStates CurrentState => currentNode.State;

        public void Update()
        {
            var transition = GetTransition();

            if (transition != null)
            {
                ChangeState(transition.To);
                foreach (var node in nodes.Values)
                {
                    ResetActionPredicateFlags(node.Transitions);
                }
                ResetActionPredicateFlags(anyTransitions);
            }

            currentNode.State?.Update();
        }

        static void ResetActionPredicateFlags(IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions.OfType<Transition<ActionPredicate>>())
            {
                transition.condition.flag = false;
            }
        }

        public void FixedUpdate()
        {
            currentNode.State?.FixedUpdate();
        }

        public void SetState(IStates state)
        {
            currentNode = nodes[state.GetType()];
            currentNode.State?.OnEnter();
        }

        void ChangeState(IStates state)
        {
            if (state == currentNode.State)
                return;

            var previousState = currentNode.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState.OnEnter();
            currentNode = nodes[state.GetType()];
        }

        public void AddTransition<T>(IStates from, IStates to, T condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition<T>(IStates to, T condition)
        {
            anyTransitions.Add(new Transition<T>(GetOrAddNode(to).State, condition));
        }

        Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Evaluate())
                    return transition;

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Evaluate())
                    return transition;
            }

            return null;
        }

        StateNode GetOrAddNode(IStates state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                nodes[state.GetType()] = node;
            }

            return node;
        }

        class StateNode
        {
            public IStates State { get; }
            public HashSet<Transition> Transitions { get; }

            public StateNode(IStates state)
            {
                State = state;
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition<T>(IStates to, T predicate)
            {
                Transitions.Add(new Transition<T>(to, predicate));
            }
        }
    }
}